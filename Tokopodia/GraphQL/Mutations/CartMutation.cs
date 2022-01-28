using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Data.BuyerProfiles;
using Tokopodia.Data.Users;
using Tokopodia.Helper;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;
using Tokopodia.SyncDataService.Dtos;
using Tokopodia.SyncDataService.Http;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [Obsolete]
  public class CartMutation
  {
    public async Task<CartStatusOutput> DeleteCartByIdAsync(
        int id,
        [Service] IUser user,
        [Service] IBuyerProfile buyerProfile,
        [Service] AppDbContext context,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
      var userId = httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;
      if (userId == null)
      {
        throw new Exception("Unauthorized.");
      }

      var userResult = await user.GetById(userId);
      if (userResult == null)
        throw new UserNotFoundException();
      var buyerResult = await buyerProfile.GetByUserId(userResult.Id);
      if (buyerResult == null)
        throw new UserNotFoundException();

      var cart = context.Carts.Where(c => c.Id == id).FirstOrDefault();
      if (cart != null)
      {
        if (cart.BuyerId != buyerResult.Id)
        {
          return new CartStatusOutput($"Data in the cart does not belong to the buyerId:{buyerResult.Id}");
        }
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
      }throw new CartNotFound();
    }

    public async Task<Cart> AddCartAsync(
        AddCartInput input,
        [Service] IUser user,
        [Service] IBuyerProfile buyerProfile,
        [Service] AppDbContext context,
        [Service] IDianterExpressDataClient _diantarExpressClient,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
      var userId = httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;
      if (userId == null)
      {
        throw new Exception("Unauthorized.");
      }

      var userResult = await user.GetById(userId);
      if (userResult == null)
        throw new UserNotFoundException();
      var buyerResult = await buyerProfile.GetByUserId(userResult.Id);
      if (buyerResult == null)
        throw new UserNotFoundException();

      var buyyer = context.BuyerProfiles.Where(c => c.Id == buyerResult.Id).FirstOrDefault();

      var product = context.Products.Where(p => p.Id == input.ProductId).FirstOrDefault();
      var seller = context.SellerProfiles.Where(s => s.Id == product.SellerId).FirstOrDefault();

      if (buyyer == null)
      {
        Console.WriteLine("Buyyer Not Found");
      }

      //Validasi input stock
      if (input.Quantity < 0 || input.Quantity >= product.Stock)
      {
        Console.WriteLine("Quantity cannot be negative");
      }

      //Validasi lat dan long seller
      if (seller.LatSeller == 0 || seller.LongSeller == 0)
      {
        Console.WriteLine("Seller has not input Lat and Long");

      }

      //Validasi Input Jasa Kirim
      if (!(input.ShippingTypeId == 1 || input.ShippingTypeId == 2 || input.ShippingTypeId == 3))
      {
        throw new ShippingNotFound();
      }


            //Validasi lat dan long buyer
            if (input.LatBuyer == 0 || input.LongBuyer == 0)
            {
                if (buyyer.latBuyer == 0 || buyyer.longBuyer == 0)
                {
                    Console.WriteLine("Buyer has not input Lat and Long");
                }
                input.LatBuyer = buyyer.latBuyer;
                input.LongBuyer = buyyer.longBuyer;

            }

            var send = new CheckFeeInput
            {
                senderLat = seller.LatSeller,
                senderLong = seller.LongSeller,
                receiverLat = input.LatBuyer,
                receiverLong = input.LongBuyer,
                weight = input.Quantity * product.Weight,
                shipmentTypeId = input.ShippingTypeId
            };

            var msg = await _diantarExpressClient.CheckFee(send);
            var fee = msg.data.fee;
            var cart = new Cart
              {
                BuyerId = buyerResult.Id,
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
                ShippingCost = fee,
                Status = "OnCart",
                Product = product,
                Buyer = buyyer,
                Seller = seller
              };

            

      var ret = context.Carts.Add(cart);
      await context.SaveChangesAsync();

      return ret.Entity;
    }

    public async Task<Cart> UpdateCartAsync(
        UpdateCartInput input,
        [Service] IUser user,
        [Service] IBuyerProfile buyerProfile,
        [Service] AppDbContext context,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
      var userId = httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;
      if (userId == null)
      {
        throw new Exception("Unauthorized.");
      }

      var userResult = await user.GetById(userId);
      if (userResult == null)
        throw new UserNotFoundException();
      var buyerResult = await buyerProfile.GetByUserId(userResult.Id);
      if (buyerResult == null)
        throw new UserNotFoundException();

      var buyyer = context.BuyerProfiles.Where(c => c.Id == buyerResult.Id).FirstOrDefault();

      var cartId = context.Carts.Where(c => c.Id == input.CartId).FirstOrDefault();

      var product = context.Products.Where(p => p.Id == cartId.ProductId).FirstOrDefault();

      if (buyyer == null)
      {
        Console.WriteLine("Buyyer Not Found");
      }

      //Validasi input stock
      if (input.Quantity < 0 || input.Quantity <= product.Stock)
      {
        Console.WriteLine("Quantity cannot be negative");
      }

      //Validasi lat dan long buyer
      /*if (input.LatBuyer == null || input.LongBuyer == null)
      {
          if (buyyer.latBuyer == 0 || buyyer.longBuyer == 0)
          {
              Console.WriteLine("Buyer has not input Lat and Long");
          }
          input.LatBuyer = buyyer.latBuyer;
          input.LongBuyer = buyyer.longBuyer;

      }*/

      var cart = new Cart
      {
        Id = input.CartId,
        Quantity = input.Quantity,
        LatBuyer = input.LatBuyer,
        LongBuyer = input.LongBuyer,
        ShippingTypeId = input.ShippingTypeId,
      };

      var ret = context.Carts.Update(cart);
      await context.SaveChangesAsync();

      return ret.Entity;
    }
  }
}
