using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.GraphQL
{
    [ExtendObjectType(Name = "Query")]
    [Obsolete]
    public class QueryProduct
    {
        public ProductSellerOutput GetProductForSeller(
        [Service] AppDbContext context,
        [Service] IHttpContextAccessor httpContextAccessor)
        {
            var sellerId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var product = context.Products.Where(o => o.SellerId == sellerId).ToListAsync();

            return new ProductSellerOutput(product);
        }

        public ProductBuyerOutput GetProductForBuyer(
        ProductBuyerInput input,
        [Service] AppDbContext context)
        {

            var products = context.Products.Where(o => o.Name.Contains(input.Name));

            if (input.MaxPrice != null)
            {
                if (input.MinPrice != null)
                {
                    var productsminmax = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price < input.MaxPrice && o.Price > input.MinPrice);
                    return new ProductBuyerOutput(productsminmax);
                }
                var productsmax = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price < input.MaxPrice);
                return new ProductBuyerOutput(productsmax);
            }

            if (input.MinPrice != null)
            {
                var productsmin = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price > input.MinPrice);
                return new ProductBuyerOutput(productsmin);
            }

            if (input.Category != null)
            {
                var productscat = context.Products.Where(o => o.Name.Contains(input.Name) && o.Category.Contains(input.Category));
                return new ProductBuyerOutput(productscat);
            }

            return new ProductBuyerOutput(products);
        }
    }
}
