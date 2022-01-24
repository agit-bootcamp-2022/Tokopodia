using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Models;

namespace Tokopodia.GraphQL
{
    public class MutationProduct
    {
        [Authorize]
        public async Task<Product> AddProductAsync(
            ProductInput input,
            [Service] AppDbContext context)
        {
            var product = new Product
            {
                Name = input.Name,
                Stock = input.Stock,
                Price = input.Price,
                Created = DateTime.Now
            };

            var ret = context.Products.Add(product);
            await context.SaveChangesAsync();

            return ret.Entity;
        }
        public async Task<Product> GetProductByIdAsync(
            int id,
            [Service] AppDbContext context)
        {
            var product = context.Products.Where(o => o.Id == id).FirstOrDefault();

            return await Task.FromResult(product);
        }
        [Authorize]
        public async Task<Product> UpdateProductAsync(
            ProductInput input,
            [Service] AppDbContext context)
        {
            var product = context.Products.Where(o => o.Id == input.Id).FirstOrDefault();
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
            [Service] AppDbContext context)
        {
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
