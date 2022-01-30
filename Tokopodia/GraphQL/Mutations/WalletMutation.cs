using AutoMapper;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;
using System;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;
using Tokopodia.Helper;
using Tokopodia.Data.Users;
using Tokopodia.Data.BuyerProfiles;
using Tokopodia.Data.SellerProfiles;
using Tokopodia.Data.Wallets;
namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [Obsolete]
  public class WalletMutation
  {
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _db;

    public WalletMutation([Service] IMapper mapper,
                          [Service] IHttpContextAccessor httpContextAccessor,
                          [Service] AppDbContext db)
    {
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
      _db = db;
    }

    [Authorize(Roles = new[] { "Buyer", "Seller" })]
    public async Task<WalletOutput> AddWalletAsync(WalletInput input, [Service] IUangTrans _uangTrans, [Service] IUser _user, [Service] IBuyerProfile _buyer, [Service] ISellerProfile _seller, [Service] IWallet _wallet)
    {
      try
      {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;
        if (userId == null)
          throw new Exception("Unauthorized.");
        var userRes = await _user.GetById(userId);
        if (userRes == null)
          throw new Exception("user not found");
        var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        var hashPassword = input.Password;
        var regisWallet = new RegisterInput
        {
          Username = input.Username,
          Password = hashPassword,
        };
        if (userRole == "Seller")
        {
          var profile = await _seller.GetByUserId(userId);
          regisWallet.Email = profile.User.Email;
          regisWallet.FirstName = profile.ShopName;
          regisWallet.LastName = profile.ShopName;
        }
        else if (userRole == "Buyer")
        {
          var profile = await _buyer.GetByUserId(userId);
          regisWallet.Email = profile.User.Email;
          regisWallet.FirstName = profile.FirstName;
          regisWallet.LastName = profile.LastName;
        }
        Console.WriteLine("==>usernmae: " + regisWallet.Username + " " + regisWallet.Password);
        var walletRegis = await _uangTrans.RegisterUser.ExecuteAsync(regisWallet);
        Console.WriteLine("==>msg: " + walletRegis.Data.RegisterUser.Message);
        if (walletRegis.Data.RegisterUser.Data.Username == null)
          throw new Exception("Username is already taken or password is weak. Try again. Failed to register new user on wallet service.");
        var insertWallet = new Wallet
        {
          Password = hashPassword,
          UangTransId = walletRegis.Data.RegisterUser.Data.Id,
          UserId = userId,
          Username = input.Username,
          User = userRes
        };
        var walletResult = await _wallet.Insert(insertWallet);
        var walletOuput = _mapper.Map<WalletOutput>(walletResult);
        return walletOuput;
      }
      catch (System.Exception ex)
      {

        throw new DataFailed(ex.Message);
      }
    }
  }
}
