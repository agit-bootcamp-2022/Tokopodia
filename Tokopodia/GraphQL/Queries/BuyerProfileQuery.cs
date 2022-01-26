using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Tokopodia.Data.BuyerProfiles;
using Tokopodia.Data.Users;
using Tokopodia.Helper;
using Tokopodia.Output;

namespace Tokopodia.GraphQL.Queries
{
  [ExtendObjectType(Name = "Query")]
  [System.Obsolete]
  public class BuyerProfileQuery
  {
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BuyerProfileQuery([Service] IMapper mapper, [Service] IHttpContextAccessor httpContextAccessor)
    {
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    [Authorize(Roles = new[] { "Buyer" })]
    public async Task<BuyerOutput> GetBuyerProfile([Service] IUser _user, [Service] IBuyerProfile _buyerProfile)
    {
      var userId = _httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;
      if (userId == null)
      {
        throw new Exception("Unauthorized.");
      }
      var userResult = await _user.GetById(userId);
      if (userResult == null)
        throw new UserNotFoundException();
      var profileResult = await _buyerProfile.GetByUserId(userResult.Id);
      if (profileResult == null)
        throw new UserNotFoundException();
      var buyerOutput = _mapper.Map<BuyerOutput>(profileResult);
      buyerOutput.userOutput = _mapper.Map<UserOutput>(profileResult.User);
      return buyerOutput;
    }

  }
}