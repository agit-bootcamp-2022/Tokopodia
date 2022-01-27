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
  public class CartMutation
  {
    public async Task<CartStatusOutput> DeleteCartByIdAsync(
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
          return new CartStatusOutput($"Data {cart.Id} successfully deleted from cart");
        }
        else
        {
          return new CartStatusOutput("Data in the cart cannot be deleted");
        }
      }
      else
      {
        return new CartStatusOutput($"Data {cart.Id} not found");
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

      if (buyyer == null)
      {
        Console.WriteLine("Buyyer Not Found");
      }

      if (input.Quantity < 0 || input.Quantity <= product.Stock)
      {
        Console.WriteLine("Quantity cannot be negative");
      }

      if (seller.LatSeller == 0 || seller.LongSeller == 0)
      {
        Console.WriteLine("Buyer has not input Lat and Long");

      }

      // if (input.LatBuyer == null || input.LongBuyer == null)
      // {
      //   if (buyyer.latBuyer == 0 || buyyer.longBuyer == 0)
      //   {
      //     Console.WriteLine("Buyer has not input Lat and Long");
      //   }
      //   input.LatBuyer = buyyer.latBuyer;
      //   input.LongBuyer = buyyer.longBuyer;

      // }

      var cart = new Cart
      {
        BuyerId = Convert.ToInt32(buyerId),
        ProductId = input.ProductId,
        SellerId = seller.Id,
        Quantity = input.Quantity,
        Weight = input.Quantity * product.Weight,
        BillingSeller = product.Price * input.Quantity,
        LatSeller = seller.LatSeller,
        LongSeller = seller.LongSeller,
        LatBuyer = input.LatBuyer,
        LongBuyer = input.LongBuyer,
        ShippingTypeId = input.ShippingTypeId,
        ShippingCost = 123213,
        Product = product
      };

      var ret = context.Carts.Add(cart);
      await context.SaveChangesAsync();

      return ret.Entity;
    }

    public async Task<Cart> UpdateCartAsync(
        UpdateCartInput input,
        [Service] AppDbContext context,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
      var buyerId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
      var buyyer = context.BuyerProfiles.Where(c => c.Id == Convert.ToInt32(buyerId)).FirstOrDefault();

      var cartId = context.Carts.Where(c => c.Id == input.CartId).FirstOrDefault();

      var product = context.Products.Where(p => p.Id == cartId.ProductId).FirstOrDefault();

      if (buyyer == null)
      {
        Console.WriteLine("Buyyer Not Found");
      }

      if (input.Quantity < 0 || input.Quantity <= product.Stock)
      {
        Console.WriteLine("Quantity cannot be negative");
      }

      // if (input.LatBuyer == null || input.LongBuyer == null)
      // {
      //   if (buyyer.latBuyer == 0 || buyyer.longBuyer == 0)
      //   {
      //     Console.WriteLine("Buyer has not input Lat and Long");
      //   }
      //   input.LatBuyer = buyyer.latBuyer;
      //   input.LongBuyer = buyyer.longBuyer;

      // }

      var cart = new Cart
      {
        Id = input.CartId,
        Quantity = input.Quantity,
        LatBuyer = input.LatBuyer,
        LongBuyer = input.LongBuyer,
        ShippingTypeId = input.ShippingTypeId,
      };

      var ret = context.Carts.Add(cart);
      await context.SaveChangesAsync();

      return ret.Entity;
    }
  }
}
