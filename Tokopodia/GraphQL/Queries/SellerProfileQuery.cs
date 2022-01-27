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
using Tokopodia.Helpers;
using Tokopodia.Output;

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

        public SellerOutput ShowProfile()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (userName == null)
            {
                throw new Exception("Silahkan Login Terlebih Dahulu");
            }
            return (SellerOutput)_context.SellerProfiles.Select(seller => new SellerOutput()
            {
                Id = Convert.ToInt32(seller.UserId),
                Username = seller.Username,
                //Address = seller.Address,
                //ShopName = seller.ShopName,
                CreatedAt = seller.CreatedAt
            }).Where(seller => seller.Username == userName);
        }



    }
}