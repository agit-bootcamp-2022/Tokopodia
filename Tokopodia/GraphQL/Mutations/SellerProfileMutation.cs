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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [System.Obsolete]
  public class SellerProfileMutation
  {
    public string SetHelloWorldSeller()
    {
      return "Hello World seller!";
    }
  }

}