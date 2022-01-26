using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Models;

namespace Tokopodia.GraphQL.Querys
{
    [ExtendObjectType(Name = "Query")]
    [Obsolete]
    public class QueryCart
    {
        public async Task<IQueryable<Cart>> GetAllCartsByBuyer(
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var buyerId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var carts = context.Carts.Where(c => c.BuyerId == Convert.ToInt32(buyerId));

            return carts; 
        }

        public async Task<IQueryable<Cart>> GetCartOnCartBuyer(
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var buyerId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cart = context.Carts.Where(c => c.BuyerId == Convert.ToInt32(buyerId) && c.Status == "OnCart");

            return cart;
        }
    }
}
