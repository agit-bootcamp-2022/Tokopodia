using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Data.SellerProfiles;
using Tokopodia.Data.Users;
using Tokopodia.Helper;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.GraphQL.Queries
{
    [ExtendObjectType(Name = "Query")]
    [Obsolete]
    public class ProductQuery
    {
        [Authorize(Roles = new[] { "Seller" })]
        public async Task<ProductSellerOutput> GetProductForSeller(
        [Service] AppDbContext context,
        [Service] IHttpContextAccessor httpContextAccessor,
        [Service] IUser user, 
        [Service] ISellerProfile sellerProfile)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;
            if (userId == null)
            {
                throw new Exception("Unauthorized.");
            }

            var userResult = await user.GetById(userId);
            if (userResult == null)
                throw new UserNotFoundException();
            var profileResult = await sellerProfile.GetByUserId(userResult.Id);
            if (profileResult == null)
                throw new UserNotFoundException();

            var product = context.Products.Where(o => o.SellerId == profileResult.Id).ToListAsync();

            return new ProductSellerOutput(product, profileResult.Id);
        }

        public async Task<ProductBuyerOutput> GetProductForBuyer(
        ProductBuyerInput input,
        Product product,
        [Service] AppDbContext context,
        [Service] ISellerProfile sellerProfile)
        {

            var products = context.Products.Where(o => o.Name.Contains(input.Name));

            var profileResult = await sellerProfile.GetByUserId(Convert.ToString(product.Id));
            if (profileResult == null)
                throw new UserNotFoundException();

            if (input.MaxPrice != null)
            {
                if (input.MinPrice != null)
                {
                    var productsminmax = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price < input.MaxPrice && o.Price > input.MinPrice);
                    return new ProductBuyerOutput(productsminmax, profileResult.Id);
                }
                var productsmax = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price < input.MaxPrice);
                return new ProductBuyerOutput(productsmax, profileResult.Id);
            }

            if (input.MinPrice != null)
            {
                var productsmin = context.Products.Where(o => o.Name.Contains(input.Name) && o.Price > input.MinPrice);
                return new ProductBuyerOutput(productsmin, profileResult.Id);
            }

            if (input.Category != null)
            {
                var productscat = context.Products.Where(o => o.Name.Contains(input.Name) && o.Category.Contains(input.Category));
                return new ProductBuyerOutput(productscat, profileResult.Id);
            }

            return new ProductBuyerOutput(products, profileResult.Id);
        }
    }
}
