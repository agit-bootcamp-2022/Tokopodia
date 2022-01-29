using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Tokopodia.Data.BuyerProfiles;
using Tokopodia.Data.Carts;
using Tokopodia.Data.Wallets;
using Tokopodia.Data.Products;
using Tokopodia.Data.Users;
using Tokopodia.Data.Transactions;
using Tokopodia.Helper;
using Tokopodia.Models;
using Tokopodia.SyncDataService.Http;
using Tokopodia.SyncDataService.GraphQLClients;
using Tokopodia.SyncDataService.Dtos;
using Tokopodia.Output;
using System.Collections.Generic;
using AutoMapper;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [Obsolete]
  public class TransactionMutation
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public TransactionMutation([Service] IHttpContextAccessor httpContextAccessor, [Service] IMapper mapper)
    {
      _httpContextAccessor = httpContextAccessor;
      _mapper = mapper;
    }

    [Authorize(Roles = new[] { "Buyer" })]
    public async Task<TransactionOutput> AddTransaction([Service] IBuyerProfile _buyerProfile,
                                             [Service] ICart _cart,
                                             [Service] IWallet _wallet,
                                             [Service] IProduct _product,
                                             [Service] IDianterExpressDataClient _diantarExpressClient,
                                             [Service] IUangTrans _uangTrans,
                                             [Service] OwnerConsumer _consume,
                                             [Service] IUser _user,
                                             [Service] ITransaction _transaction)
    {
      try
      {

        var profileId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst("ProfileId").Value);
        var profileResult = await _buyerProfile.GetProfileById(profileId);
        if (profileResult == null)
          throw new Exception("Unauthorized.");
        // cek semu cart list yang statusnya OnCart
        var carts = await _cart.GetAllByStatusOnCartAndBuyerId(profileResult.Id);

        // hitung SumBillingSeller
        // hitung SumShippingCost
        var sumBillingSeller = 0.0;
        var sumShippingCost = 0.0;
        foreach (var cart in carts)
        {
          sumBillingSeller += cart.BillingSeller;
          sumShippingCost += cart.ShippingCost;
        }
        // TotalBilling
        var totalBilling = sumBillingSeller + sumShippingCost;
        // cek saldo dulu ke wallet service -> ambil dengan id user => login => get saldo
        var walletuser = await _wallet.GetByUserId(profileResult.User.Id);
        if (walletuser == null)
          throw new Exception("Wallet user not found.");
        // login ke wallet service
        var loginResult = await _uangTrans.LoginUser.ExecuteAsync(new LoginUserInput { Username = walletuser.Username, Password = walletuser.Password });
        if (loginResult.Data.LoginUser.Token == null)
          throw new Exception("Invalid Username/password. Error fetch token from wallet service. ");
        loginResult = await _uangTrans.LoginUser.ExecuteAsync(new LoginUserInput { Username = walletuser.Username, Password = walletuser.Password });
        Console.WriteLine("token: " + loginResult.Data.LoginUser.Token);
        // get saldo for buyer
        var saldo = await _consume.GetSaldo(loginResult.Data.LoginUser.Token);
        if (loginResult.Data.LoginUser.Token == null)
          throw new Exception("Error fetch saldo from wallet service.");
        Console.WriteLine("balance: " + saldo.balance);
        // check saldi dengan total billing, jika kurang return erro
        if (totalBilling > saldo.balance)
          throw new Exception("Saldo wallet is not enought.");
        // update stok product ke database
        foreach (var cart in carts)
        {
          cart.Product.Stock = cart.Product.Stock - cart.Quantity;
          if (cart.Product.Stock < 0)
            throw new Exception($"Stock {cart.Product.Name} is not ennough.");
          cart.Status = "OnTransaction";
          var updateResult = await _product.Update(cart.Product);
          cart.Product = updateResult;
          var cartUpdate = await _cart.Update(cart);

        }

        IList<SellerCreateInput> sellers = new List<SellerCreateInput>();
        foreach (var cart in carts)
        {
          var sellerWallet = await _wallet.GetByUserId(cart.Seller.UserId);
          sellers.Add(new SellerCreateInput { amountSeller = (float)cart.BillingSeller, sellerId = sellerWallet.UangTransId });
        }

        //create transaction
        var tranInput = new TransactionCreate
        {
          amountBuyer = (float)totalBilling,
          amountCourier = (float)sumShippingCost,
          buyerId = walletuser.UangTransId,
          sellers = sellers
        };
        var walletTransaction = await _consume.CreateTransaction(tranInput, loginResult.Data.LoginUser.Token);
        if (walletTransaction.message != "Success")
          throw new Exception("Failed to create transaction on wallet service");

        var courier = await _user.Authenticate(new Input.LoginInput { Username = "courier1", Password = "Courier1!" }, "Courier");
        // simpan semuanya di transaction
        var transactionInput = new Transaction
        {
          Address = profileResult.Address,
          Carts = carts,
          CreatedAt = DateTime.Now,
          status = TransactionStatus.Paid,
          SumBillingSeller = sumBillingSeller,
          SumShippingCost = sumShippingCost,
          Token = courier.Token,
          TotalBilling = totalBilling,
          WalletTransactionId = walletTransaction.transactionId
        };
        var transaction = await _transaction.Insert(transactionInput);

        // create shipping ke anter express dengan for each
        IList<Cart> cartsObj = new List<Cart>();
        foreach (var cart in carts)
        {
          var shipmentCreate = new ShipmentInput
          {
            receiverContact = cart.Buyer.Address,
            receiverLat = cart.LatBuyer,
            receiverLong = cart.LongBuyer,
            receiverName = $"{cart.Buyer.FirstName} {cart.Buyer.LastName}",
            senderContact = cart.Seller.Address,
            senderLat = cart.LatSeller,
            senderLong = cart.LongSeller,
            senderName = cart.Seller.ShopName,
            shipmentTypeId = cart.ShippingTypeId,
            totalWeight = cart.Weight,
            transactionId = transaction.TransactionId,
            token = courier.Token
          };
          var shippingResult = await _diantarExpressClient.CreateShipment(shipmentCreate);
          if (shippingResult.status != "success")
            throw new Exception("Failed to create Shipment on Diantar Express Serivce");
          cart.TransactionId = transaction.TransactionId;
          cart.ShippingId = shippingResult.data.shipmentId;
          var cartUpdate = await _cart.Update(cart);
          cartsObj.Add(cartUpdate);
        }
        transaction.Carts = cartsObj;
        var transactionUpdate = await _transaction.Update(transaction);
        var transOutput = _mapper.Map<TransactionOutput>(transactionUpdate);
        transOutput.CartsOutput = _mapper.Map<IEnumerable<CartOutput>>(transactionUpdate.Carts);
        return transOutput;
      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }
    }

    [Authorize(Roles = new[] { "Courier" })]
    public async Task<Message> UpdateTransaction(UpdateInput input, [Service] ITransaction _transaction)
    {
      string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
      string[] tokenWords = token.Split(' ');
      var transactionResult = await _transaction.GetById(input.transactionId);
      if (tokenWords[1] != transactionResult.Token)
        return new Message { message = "fail" };
      transactionResult.status = TransactionStatus.Completed;
      var updateTransaction = await _transaction.Update(transactionResult);
      return new Message { message = "success" };
    }

  }
}