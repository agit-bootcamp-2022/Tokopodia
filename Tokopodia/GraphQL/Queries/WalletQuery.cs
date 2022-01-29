using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Data.BuyerProfiles;
using Tokopodia.Data.SellerProfiles;
using Tokopodia.Data.Users;
using Tokopodia.Data.Wallets;
using Tokopodia.Models;
using Tokopodia.Output;
using Tokopodia.SyncDataService.GraphQLClients;
using Tokopodia.SyncDataService.Http;

namespace Tokopodia.GraphQL.Queries
{
    public class WalletQuery
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _db;

        public WalletQuery([Service] IMapper mapper,
                              [Service] IHttpContextAccessor httpContextAccessor,
                              [Service] AppDbContext db)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        [Authorize(Roles = new[] { "Buyer", "Seller" })]
        public async Task<BalanceOutput> GetUserWalletAsync([Service] IUangTrans _uangTrans,
                                                            [Service] IBuyerProfile _buyer, 
                                                            [Service] ISellerProfile _seller, 
                                                            [Service] IWallet _wallet,
                                                            [Service] OwnerConsumer _consume)
        {
            var buyerId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst("ProfileId").Value);
            var buyerProfileResult = await _buyer.GetProfileById(buyerId);

            var sellerId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst("ProfileId").Value);
            var sellerProfileResult = await _seller.GetProfileById(sellerId);

            if (buyerProfileResult == null || sellerProfileResult == null)
                throw new Exception("Unauthorized.");

            // cek saldo dulu ke wallet service -> ambil dengan id user => login => get saldo
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (userRole == "Seller")
            {
                var walletuser = await _wallet.GetByUserId(sellerProfileResult.User.Id);
                if (walletuser == null)
                    throw new Exception("Wallet user not found.");
                // login ke wallet service
                var loginResult = await _uangTrans.LoginUser.ExecuteAsync(new LoginUserInput { Username = walletuser.Username, Password = walletuser.Password });
                if (loginResult.Data.LoginUser.Token == null)
                    throw new Exception("Invalid Username/password. Error fetch token from wallet service. ");
                // var loginResult = await _uangTrans.LoginUser.ExecuteAsync(new LoginUserInput { Username = "test", Password = "Kosongkan@Saja1" });
                Console.WriteLine("token: " + loginResult.Data.LoginUser.Token);
                // get saldo for buyer
                var saldo = await _consume.GetSaldo(loginResult.Data.LoginUser.Token);
                if (loginResult.Data.LoginUser.Token == null)
                    throw new Exception("Error fetch saldo from wallet service.");

                var balanceOutput = new BalanceOutput
                {
                    UangTransId = saldo.customerId.ToString(),
                    Balance = saldo.balance
                };

                return balanceOutput;
            }
            else if (userRole == "Buyer")
            {
                var walletuser = await _wallet.GetByUserId(buyerProfileResult.User.Id);
                if (walletuser == null)
                    throw new Exception("Wallet user not found.");
                // login ke wallet service
                var loginResult = await _uangTrans.LoginUser.ExecuteAsync(new LoginUserInput { Username = walletuser.Username, Password = walletuser.Password });
                if (loginResult.Data.LoginUser.Token == null)
                    throw new Exception("Invalid Username/password. Error fetch token from wallet service. ");
                // var loginResult = await _uangTrans.LoginUser.ExecuteAsync(new LoginUserInput { Username = "test", Password = "Kosongkan@Saja1" });
                Console.WriteLine("token: " + loginResult.Data.LoginUser.Token);
                // get saldo for buyer
                var saldo = await _consume.GetSaldo(loginResult.Data.LoginUser.Token);
                if (loginResult.Data.LoginUser.Token == null)
                    throw new Exception("Error fetch saldo from wallet service.");

                var balanceOutput = new BalanceOutput
                {
                    UangTransId = saldo.customerId.ToString(),
                    Balance = saldo.balance
                };

                return balanceOutput;
            } 
            else
            {
                throw new Exception("Unauthorized.");
            }
        }
    }
}
