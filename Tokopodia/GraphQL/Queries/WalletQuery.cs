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
        public WalletOutput GetUserWallet()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var wallet = _db.Wallets.Where(wallet => wallet.UserId == userId);
            var walletRead = _mapper.Map<WalletOutput>(wallet);

            return walletRead;
        }
    }
}
