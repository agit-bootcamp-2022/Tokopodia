using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tokopodia.Data.BuyerProfiles;
using Tokopodia.Data.Users;
using Tokopodia.Dto;
using Tokopodia.Helper;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [System.Obsolete]
  public class BuyerProfileMutation
  {
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BuyerProfileMutation([Service] IMapper mapper, [Service] IHttpContextAccessor httpContextAccessor)
    {
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BuyerOutput> RegisterBuyerProfile(RegisterBuyerInput register, [Service] IUser _user, [Service] IBuyerProfile _buyerProfile)
    {
      try
      {
        var userResult = await _user.Registration(new IdentityUser { UserName = register.Username, Email = register.Email, PasswordHash = register.Password });
        var roleResult = await _user.UpdateRole(userResult, "Buyer");
        var buyerObj = _mapper.Map<BuyerProfile>(register);
        buyerObj.CreatedAt = DateTime.Now;
        buyerObj.UserId = userResult.Id;
        buyerObj.User = userResult;
        var profileResult = await _buyerProfile.Insert(buyerObj);
        var buyerOutput = _mapper.Map<BuyerOutput>(profileResult);
        buyerOutput.userOutput = _mapper.Map<UserOutput>(profileResult.User);
        return buyerOutput;
      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }
    }

    public async Task<LoginOutput> Authenticate(LoginInput input, [Service] IUser _user, [Service] IBuyerProfile _buyerProfile)
    {
      try
      {
        var result = await _user.Authenticate(input, "Buyer");
        return result;
      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }
    }

    [Authorize(Roles = new[] { "Buyer" })]
    public async Task<BuyerOutput> SetBuyerPosition(BuyerPositionInput input, [Service] IUser _user, [Service] IBuyerProfile _buyerProfile)
    {
      try
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

        profileResult.latBuyer = input.LatBuyer;
        profileResult.longBuyer = input.LongBuyer;
        var updateResult = await _buyerProfile.Update(profileResult);
        var buyerOutput = _mapper.Map<BuyerOutput>(updateResult);
        buyerOutput.userOutput = _mapper.Map<UserOutput>(profileResult.User);
        return buyerOutput;
      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }
    }
  }

}