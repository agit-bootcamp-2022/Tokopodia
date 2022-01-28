using AutoMapper;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Models;
using Tokopodia.Output;
using Tokopodia.SyncDataService.Http;

namespace Tokopodia.GraphQL.Queries
{
    public class WalletQuery
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _db;
        private readonly IUangTransDataClient _uangtrans;

        public WalletQuery([Service] IMapper mapper,
                              [Service] IHttpContextAccessor httpContextAccessor,
                              [Service] AppDbContext db,
                              [Service] IUangTransDataClient uangTransDataClient)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
            _uangtrans = uangTransDataClient;
        }
        public async Task<BalanceOutput> GetUserWalletAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var wallet = _db.Wallets.Where(wallet => wallet.UserId == userId);

            var walletRead = await _uangtrans.GetSaldoUser();

            return walletRead;
        }
    }
}
