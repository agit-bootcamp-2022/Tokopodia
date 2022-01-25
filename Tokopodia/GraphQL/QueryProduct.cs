using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Input;
using Tokopodia.Models;

namespace Tokopodia.GraphQL
{
    public class QueryProduct
    {
        public IQueryable<Product> GetProductForSellerAsync(
        [Service] AppDbContext context,
        [Service] IHttpContextAccessor httpContextAccessor)
        {
            var sellerId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var product = context.Products.Where(o => o.SellerId == sellerId);

            return product;
        }

        public IQueryable<Product> GetProductForBuyerAsync(
        ProductBuyerInput input,
        [Service] AppDbContext context)
        {

            var products = context.Products.Where(o => o.Name.Contains(input.Name));

            if (input.MaxPrice != null)
            {
                if (input.MinPrice != null)
                {
                    var productsminmax = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price < input.MaxPrice && o.Price > input.MinPrice);
                    return productsminmax;
                }
                var productsmax = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price < input.MaxPrice);
                return productsmax;
            }

            if (input.MinPrice != null)
            {
                var productsmin = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price < input.MinPrice);
                return productsmin;
            }

            if (input.Category != null)
            {
                var productscat = context.Products.Where(o => o.Name.Contains(input.Name) && o.Category.Contains(input.Category));
                return productscat;
            }

            return products;
        }
    }
}
