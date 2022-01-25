using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
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
    public class MutationProduct
    {
        [Authorize]
        public async Task<Product> AddProductAsync(
            ProductInput input,
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var sellerId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var product = new Product
            {
                SellerId = sellerId,
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

        [Authorize]
        public async Task<Product> UpdateProductAsync(
            int id,
            ProductInput input,
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var sellerId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var seller = context.Products.Where(o => o.SellerId == sellerId).FirstOrDefault();

            var product = context.Products.Where(o => o.Id == id).FirstOrDefault();
            if (product != null)
            {
                product.Name = input.Name;
                product.Stock = input.Stock;
                product.Price = input.Price;

                context.Products.Update(product);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(product);
        }

        [Authorize]
        public async Task<Product> DeleteProductByIdAsync(
            int id,
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var sellerId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var seller = context.Products.Where(o => o.SellerId == sellerId).FirstOrDefault();

            var product = context.Products.Where(o => o.Id == id).FirstOrDefault();
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(product);
        }
    }
}
