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
using Tokopodia.Models;

namespace Tokopodia.GraphQL.Queries
{
  [ExtendObjectType(Name = "Query")]
  [Obsolete]
  public class CartQuery
  {

    public async Task<IQueryable<Cart>> GetAllCartsByBuyer(
        [Service] AppDbContext context,
        [Service] IUser user,
        [Service] IBuyerProfile buyerProfile,
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

      var carts = context.Carts.Where(c => c.BuyerId == buyerResult.Id && c.Status == "OnTransaction");

      return carts;
    }

    public async Task<IQueryable<Cart>> GetCartOnCartBuyer(
        [Service] AppDbContext context,
        [Service] IUser user,
        [Service] IBuyerProfile buyerProfile,
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

      var cart = context.Carts.Where(c => c.BuyerId == buyerResult.Id && c.Status == "OnCart");

      return cart;
    }
  }
}
