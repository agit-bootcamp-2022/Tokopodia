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
using Tokopodia.Data;
using Tokopodia.Data.SellerProfiles;
using Tokopodia.Data.Users;
using Tokopodia.Helper;
using Tokopodia.Helpers;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [System.Obsolete]
    public class SellerProfileMutation
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SellerProfileMutation([Service] IMapper mapper, [Service] IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SellerOutput> RegisterSellerProfile(RegisterSellerInput register, [Service] IUser _user, [Service] ISellerProfile _sellerProfile)
        {
            try
            {
                var userResult = await _user.Registration(new IdentityUser { UserName = register.Username, Email = register.Email, PasswordHash = register.Password });
                var roleResult = await _user.UpdateRole(userResult, "Seller");
                var sellerObj = _mapper.Map<SellerProfile>(register);
                sellerObj.CreatedAt = DateTime.Now;
                sellerObj.UserId = userResult.Id;
                sellerObj.User = userResult;
                var profileResult = await _sellerProfile.Insert(sellerObj);
                var sellerOutput = _mapper.Map<SellerOutput>(profileResult);
                sellerOutput.userOutput = _mapper.Map<UserOutput>(profileResult.User);
                return sellerOutput;
            }
            catch (System.Exception ex)
            {
                throw new DataFailed(ex.Message);
            }
        }

        public async Task<LoginOutput> AuthenticateSeller(LoginInput input, [Service] IUser _user, [Service] ISellerProfile _sellerProfile)
        {
            try
            {
                var result = await _user.Authenticate(input);
                return result;
            }
            catch (System.Exception ex)
            {
                throw new DataFailed(ex.Message);
            }
        }

        [Authorize(Roles = new[] { "Seller" })]
        public async Task<SellerOutput> SetSellerPosition(SellerPositionInput input, [Service] IUser _user, [Service] ISellerProfile _sellerProfile)
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
                var profileResult = await _sellerProfile.GetByUserId(userResult.Id);
                if (profileResult == null)
                    throw new UserNotFoundException();

                profileResult.LatSeller = input.LatSeller;
                profileResult.LongSeller = input.LongSeller;
                var updateResult = await _sellerProfile.Update(profileResult);
                var sellerOutput = _mapper.Map<SellerOutput>(updateResult);
                sellerOutput.userOutput = _mapper.Map<UserOutput>(profileResult.User);
                return sellerOutput;
            }
            catch (System.Exception ex)
            {
                throw new DataFailed(ex.Message);
            }
        }
    }

}