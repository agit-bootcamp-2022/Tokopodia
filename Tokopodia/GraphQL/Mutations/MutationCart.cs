using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    [Obsolete]
    public class MutationCart
    {
        public async Task<Status> DeleteCartByIdAsync(
            int id,
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var buyerId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var buyyer = context.Carts.Where(c => c.BuyerId == Convert.ToInt32(buyerId)).FirstOrDefault();

            var cart = context.Carts.Where(c => c.Id == id && c.BuyerId == Convert.ToInt32(buyerId)).FirstOrDefault();
            if (cart != null)
            {
                if (cart.Status == "OnCart")
                {
                    context.Carts.Remove(cart);
                    await context.SaveChangesAsync();
                    return new Status($"Data {cart.Id} successfully deleted from cart");
                }
                else
                {
                    return new Status("Data in the cart cannot be deleted");
                }
            }
            else 
            {
                return new Status($"Data {cart.Id} not found");
            }
        }

        public async Task<Cart> AddCartAsync(
            AddCartInput input,
            [Service] AppDbContext context,
            [Service] IHttpContextAccessor httpContextAccessor)
        {
            var buyerId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var buyyer = context.BuyerProfiles.Where(c => c.Id == Convert.ToInt32(buyerId)).FirstOrDefault();

            var product = context.Products.Where(p => p.Id == input.ProductId).FirstOrDefault();
            var seller = context.SellerProfiles.Where(s => s.Id == product.SellerId).FirstOrDefault();

            if (input.Quantity < 0)
            {
                Console.WriteLine("Quantity cannot be negative");
            }

            if (buyyer == null)
            {
                /*return ErrorBuilder.New()
                    .SetMessage("Buyer cant be found")
                    .SetCode("FOO_BAR")
                    .Build();*/
            }
            var cart = new Cart
            {
                BuyerId = Convert.ToInt32(buyerId),
                ProductId = input.ProductId,
                SellerId = seller.Id,
                Quantity = input.Quantity,
                BillingSeller = Convert.ToInt32(product.Price)*input.Quantity,
                LatSeller = seller.LatSeller,
                LongSeller = seller.LongSeller,
                LatBuyer = input.LatBuyer,
                LongBuyer = input.LongBuyer,
                ShippingType = input.ShippingType,
                ShippingCost = 123213
            };

            var ret = context.Carts.Add(cart);
            await context.SaveChangesAsync();

            return ret.Entity;
        }
    }
}
