using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
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
    [ExtendObjectType(Name = "Mutation")]
    [Obsolete]
    public class MutationProduct
    {
        [Authorize]
        public async Task<Product> AddProductAsync(
            ProductSellerInput input,
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var sellerId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var seller = context.Sellers.Where(o => o.SellerId == sellerId).FirstOrDefault();

            if (sellerId == null)
            {
                Console.WriteLine("Seller tidak ditemukan");
            }
            //if (seller.LatSeller == null || seller.LongSeller == null)
            //{
            //    Console.WriteLine("Seller belum input lokasi")
            //}
            var product = new Product
            {
                SellerId = sellerId,
                Name = input.Name,
                Category = input.Category,
                Description = input.Description,
                Stock = Math.Abs(input.Stock),
                Price = Math.Abs(input.Price),
                Weight = Math.Abs(input.Weight),
                Created = DateTime.Now
            };

            var ret = context.Products.Add(product);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        [Authorize]
        public async Task<Product> UpdateProductAsync(
            int id,
            ProductSellerInput input,
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var sellerId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var seller = context.Products.Where(o => o.SellerId == sellerId).FirstOrDefault();

            var product = context.Products.Where(o => o.Id == id).FirstOrDefault();
            if (product != null)
            {
                product.Name = input.Name;
                product.Stock = Math.Abs(input.Stock);
                product.Price = Math.Abs(input.Price);
                product.Weight = Math.Abs(input.Weight);

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

            //var transaction = context.Transactions.Where(o => o.productId == id).FirstOrDefault();

            var product = context.Products.Where(o => o.Id == id).FirstOrDefault();
            if (product != null)
            {
                //if (transaction == null)
                //{
                //    context.Products.Remove(product);
                //    await context.SaveChangesAsync();
                //}
                //else { Console.WriteLine("Product in Transaction"); }
            }
            else { Console.WriteLine("Product not found"); }

            return await Task.FromResult(product);
        }
    }
}
