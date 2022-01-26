using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tokopodia.Helpers;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Data.BuyerProfiles
{
  public class BuyerProfileDAL : IBuyerProfile
  {
    private UserManager<IdentityUser> _userManager;

    private RoleManager<IdentityRole> _roleManager;
    private AppSettings _appSettings;
    private readonly AppDbContext _db;

    public BuyerProfileDAL([Service] UserManager<IdentityUser> userManager,
                   [Service] RoleManager<IdentityRole> roleManager,
                   [Service] IOptions<AppSettings> appSettings,
                   [Service] AppDbContext db)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _appSettings = appSettings.Value;
      _db = db;
    }

    public Task<BuyerProfile> GetProfileById(int profileId)
    {
      throw new NotImplementedException();
    }

    public async Task<BuyerProfile> Insert(BuyerProfile profile)
    {
      var result = await _db.BuyerProfiles.AddAsync(profile);
      await _db.SaveChangesAsync();
      return result.Entity;
    }
  }
}