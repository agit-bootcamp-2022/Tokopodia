using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Tokopodia.Data;
using Tokopodia.Data.SellerProfiles;
using Tokopodia.Data.Users;
using Tokopodia.Helper;
using Tokopodia.Helpers;
using Tokopodia.Output;

namespace Tokopodia.GraphQL.Queries
{
  [ExtendObjectType(Name = "Query")]
  [System.Obsolete]
  public class SellerProfileQuery
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SellerProfileQuery([Service] IMapper mapper, [Service] IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = new[] { "Seller" })]
        public async Task<SellerOutput> GetSellerProfile([Service] IUser _user, [Service] ISellerProfile _buyerProfile)
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
            var sellerOutput = _mapper.Map<SellerOutput>(profileResult);
            sellerOutput.userOutput = _mapper.Map<UserOutput>(profileResult.User);
            return sellerOutput;
        }



    }
}