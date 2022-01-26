using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Tokopodia.Data;
using Tokopodia.Dto;
using Tokopodia.Helpers;

namespace Tokopodia.GraphQL.Queries
{
  [ExtendObjectType(Name = "Query")]
  [System.Obsolete]
  public class SellerProfileQuery
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SellerProfileQuery(AppDbContext context,
                                 IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public SellerDto ShowProfile()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (userName == null)
            {
                throw new Exception("Silahkan Login Terlebih Dahulu");
            }
            return (SellerDto)_context.SellerProfiles.Select(seller => new SellerDto()
            {
                UserId = seller.UserId,
                Username = seller.Username,
                Address = seller.Address,
                ShopName = seller.ShopName,
                CreatedAt = seller.CreatedAt
            }).Where(seller => seller.Username == userName);
        }



    }
}