using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tokopodia.Data.User;
using Tokopodia.Helper;
using Tokopodia.Input;
using Tokopodia.Models;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [System.Obsolete]
  public class BuyerProfileMutation
  {

    // TODO: mbuat utk register, login, dan set position
    public async Task<IdentityUser> RegisterBuyerProfile(RegisterBuyerInput register, [Service] IUser _user)
    {
      //register dulu ke identity
      try
      {
        var result = await _user.Registration(new IdentityUser { UserName = register.Username, Email = register.Email, PasswordHash = register.Password });
        Console.WriteLine(result.Id);
        return result;
      }
      catch (System.Exception ex)
      {
        throw new RegistrationFailed(ex.Message);
      }
      //register ke buyer profile
      //returnnya id buyer profile dan seisinya
    }
  }

}