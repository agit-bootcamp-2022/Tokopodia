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
using Tokopodia.Output;

namespace Tokopodia.Data.Users
{
  public class UserDAL : IUser
  {
    private UserManager<IdentityUser> _userManager;

    private RoleManager<IdentityRole> _roleManager;
    private AppSettings _appSettings;

    public UserDAL([Service] UserManager<IdentityUser> userManager,
                   [Service] RoleManager<IdentityRole> roleManager,
                   [Service] IOptions<AppSettings> appSettings)
    {
      _userManager = userManager;
      _roleManager = roleManager;
      _appSettings = appSettings.Value;
    }
    public async Task<LoginOutput> Authenticate(LoginInput user)
    {
      var account = await _userManager.FindByNameAsync(user.Username);
      if (account == null)
      {
        throw new Exception("Invalid username or password.");
      }
      var userFind = await _userManager.CheckPasswordAsync(
             account, user.Password);
      if (!userFind)
      {
        throw new Exception("Invalid username or password.");
      }
      List<Claim> claims = new List<Claim>();
      claims.Add(new Claim(ClaimTypes.Name, account.UserName));
      claims.Add(new Claim("UserId", account.Id.ToString()));
      var roles = await GetRolesFromUser(account.UserName);
      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(3),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature)

      };
      var output = new LoginOutput
      {
        Id = account.Id,
        Username = account.UserName
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      output.Token = tokenHandler.WriteToken(token);
      return output;
    }

    public async Task<IdentityUser> Registration(IdentityUser user)
    {
      var result = await _userManager.CreateAsync(user, user.PasswordHash);
      if (!result.Succeeded)
      {
        StringBuilder errMsg = new StringBuilder(String.Empty);
        foreach (var err in result.Errors)
        {
          errMsg.Append(err.Description + " ");
        }
        throw new Exception($"{errMsg}");
      }
      return user;
    }
    public async Task<List<string>> GetRolesFromUser(string username)
    {
      List<string> roles = new List<string>();
      var user = await _userManager.FindByNameAsync(username);
      if (user == null)
        throw new Exception($"User {username} not found");
      var results = await _userManager.GetRolesAsync(user);
      foreach (var result in results)
      {
        roles.Add(result);
      }
      return roles;
    }

    public async Task<IdentityUser> UpdateRole(IdentityUser user, string role)
    {
      var result = await _userManager.FindByIdAsync(user.Id);
      await _userManager.AddToRoleAsync(result, role);
      return result;
    }

    public async Task<IdentityUser> GetById(string UserId)
    {
      var result = await _userManager.FindByIdAsync(UserId);
      return result;
    }
  }
}