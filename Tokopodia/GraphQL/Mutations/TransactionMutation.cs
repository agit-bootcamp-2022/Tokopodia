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
using Tokopodia.Helper;
using Tokopodia.Models;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [Obsolete]
  public class TransactionMutation
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionMutation([Service] IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    [Authorize(Roles = new[] { "Buyer" })]
    public async Task<string> AddTransaction([Service] IBuyerProfile _buyerProfile,
                                             [Service] ICart _cart,
                                             [Service] IWallet _wallet,
                                             [Service] IProduct _product)
    {
      try
      {
        var profileId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst("ProfileId").Value);
        var profileResult = await _buyerProfile.GetProfileById(profileId);
        if (profileResult == null)
          throw new Exception("Unauthorized.");
        // cek semu cart list yang statusnya OnCart
        var carts = await _cart.GetAllByStatusOnCart();
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
        var walletuser = _wallet.GetByUserId(profileResult.User.Id);
        // login ke wallet service
        // get saldo for buyer
        // check saldi dengan total billing, jika kurang return erro
        // update stok product ke database
        // foreach (var cart in carts)
        // {
        //   cart.Product.Stock = cart.Product.Stock - 
        // }
        // var product = context.Products.Where(o => o.Id == productId).FirstOrDefault();
        // if (product != null)
        // {
        //   product.Name = input.Name;
        //   product.Stock = input.Stock;
        //   product.Price = input.Price;
        //   product.Weight = input.Weight;

        //   context.Products.Update(product);
        //   await context.SaveChangesAsync();
        // }
        // simpan semuanya di transaction
        var transactionInput = new Transaction
        {
          Address = profileResult.Address,
          Carts = carts,
          CreatedAt = DateTime.Now,
          status = TransactionStatus.Paid, //tambahin nanti
          SumBillingSeller = 10, //tambahin nanti
          SumShippingCost = 10, //tambahin nanti
          Token = "test", //tambahin nanti
          TotalBilling = 20, //tamhain nanti
          WalletTransactionId = 1 //tambahin nanti
        };

        return "test";

      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }

    }
  }
}