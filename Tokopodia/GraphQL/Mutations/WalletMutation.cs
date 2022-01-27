using AutoMapper;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.GraphQL.Mutations
{
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

        public async Task<WalletOutput> AddWalletAsync(WalletInput input)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var wallet = new Wallet
            {
                UserId = userId,
                Username = input.Username,
                Password = input.Password
            };

            _db.Wallets.Add(wallet);
            await _db.SaveChangesAsync();

            return await Task.FromResult(new WalletOutput
            {
                WalletId = wallet.WalletId,
                UserId = userId,
                Username = wallet.Username
            });

        }
    }
}
