using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Data.SellerProfiles;
using Tokopodia.Data.Users;
using Tokopodia.Helper;
using Tokopodia.Input;
using Tokopodia.Models;

namespace Tokopodia.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    [Obsolete]
    public class ProductMutation
    {
        [Authorize(Roles = new[] { "Seller" })]
        public async Task<Product> AddProductAsync(
            ProductSellerInput input,
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

            if (profileResult.LatSeller == 0 || profileResult.LongSeller == 0)
            {
                Console.WriteLine("Seller belum input lokasi");
            }
            if (input.Stock < 0 || input.Price < 0 || input.Weight < 0)
            {
                Console.WriteLine("Value cannot be negative");
            }
            var product = new Product
            {
                SellerId = profileResult.Id,
                Name = input.Name,
                Category = input.Category,
                Description = input.Description,
                Stock = input.Stock,
                Price = input.Price,
                Weight = input.Weight,
                Created = DateTime.Now
            };

            var ret = context.Products.Add(product);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        [Authorize(Roles = new[] { "Seller" })]
        public async Task<Product> UpdateProductAsync(
            int productId,
            ProductSellerInput input,
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

            var seller = context.Products.Where(o => o.SellerId == profileResult.Id).FirstOrDefault();

            if (input.Stock < 0 || input.Price < 0 || input.Weight < 0)
            {
                Console.WriteLine("Value cannot be negative");
            }

            var product = context.Products.Where(o => o.Id == productId).FirstOrDefault();
            if (product != null)
            {
                product.Name = input.Name;
                product.Stock = input.Stock;
                product.Price = input.Price;
                product.Weight = input.Weight;

                context.Products.Update(product);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(product);
        }

        [Authorize(Roles = new[] { "Seller" })]
        public async Task<Product> DeleteProductByIdAsync(
            int productId,
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

            var seller = context.Products.Where(o => o.SellerId == profileResult.Id).FirstOrDefault();

            var cart = context.Carts.Where(o => o.ProductId == productId && o.Status == "OnCart").FirstOrDefault();

            var product = context.Products.Where(o => o.Id == productId).FirstOrDefault();
            if (product != null)
            {
                if (cart != null)
                {
                    context.Carts.Remove(cart);
                    await context.SaveChangesAsync();
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
            else { Console.WriteLine("Product not found"); }

            return await Task.FromResult(product);
        }
    }
}
