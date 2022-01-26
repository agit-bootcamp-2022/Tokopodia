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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tokopodia.Data;
using Tokopodia.Dto;
using Tokopodia.Helpers;
using Tokopodia.Models;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [System.Obsolete]
  public class SellerProfileMutation
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AppSettings _appSettings;

        public SellerProfileMutation(AppDbContext context,
                                 IHttpContextAccessor httpContextAccessor, 
                                 UserManager<IdentityUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IOptions<AppSettings> appSetings)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSetings.Value;
        }

        public async Task<SellerProfileDto> RegisterSellerAsync(RegisterSellerInput input)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync("Seller");
                if (!roleExist.Equals(false))
                {
                    var newUser = new IdentityUser
                    {
                        UserName = input.Username,
                        Email = input.Username

                    };

                    var result = await _userManager.CreateAsync(newUser, input.Password);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Gagal Menambahkan User");
                    }

                    var userResult = await _userManager.FindByNameAsync(newUser.Email);
                    await _userManager.AddToRoleAsync(userResult, "Seller");

                    var userEntity = new SellerProfile
                    {
                        UserId = userResult.Id,
                        Username = input.Username,
                        ShopName = input.ShopName,
                        Address = input.Address,
                        CreatedAt = DateTime.Now,
                        LatSeller = 0,
                        LongSeller = 0
                    };

                    var ret1 = _context.Users.Add(newUser);
                    var ret2 = _context.SellerProfiles.Add(userEntity);
                    await _context.SaveChangesAsync();

                    return await Task.FromResult(new SellerProfileDto
                    {
                        UserId = userResult.Id,
                        Username = input.Username,
                        ShopName = input.ShopName,
                        Address = input.Address
                    });
                }
                else
                {
                    await AddRole("Seller");
                    throw new Exception($"Role Driver Belum Ada --> Menambhakan Role Driver");
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<SellerTokenDto> LoginAsync(SellerLoginInput input)
        {
            try
            {
                var userFind = await _userManager.CheckPasswordAsync(await _userManager.FindByNameAsync(input.Username), input.Password);
                if (!userFind)
                {
                    throw new Exception("Username or Passwor Invalid");
                }

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, input.Username));
                var roles = await GetRolesFromUser(input.Username);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var user = new SellerTokenDto
                {
                    username = input.Username
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Expired = tokenDescriptor.Expires.ToString();
                user.Token = tokenHandler.WriteToken(token);
                return user;

            }
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}");
            }
        }

        public SellerPositionDto SetAddressPosition(SellerPositionInput input)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (userName == null)
            {
                throw new Exception("Silahkan Login Terlebih Dahulu");
            }
            var result = _context.SellerProfiles.FirstOrDefault(dri => dri.Username == userName);

            result.LatSeller = input.LatSeller;
            result.LongSeller = input.LongSeller;
            _context.SaveChanges();

            var seller = new SellerPositionDto()
            {
                ShopName = result.ShopName,
                Address = result.Address,
                LatSeller = input.LatSeller,
                LongSeller = input.LongSeller
            };

            return seller;
        }

        public async Task AddRole(string rolename)
        {
            try
            {
                var roleIsExist = await _roleManager.RoleExistsAsync(rolename);
                if (roleIsExist)
                {
                    throw new Exception($"Role {rolename} sudah terdaftar");
                }

                else
                    await _roleManager.CreateAsync(new IdentityRole(rolename));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> GetRolesFromUser(string username)
        {
            try
            {
                List<string> lstRoles = new List<string>();
                var user = await _userManager.FindByEmailAsync(username);
                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    lstRoles.Add(role);
                }
                return lstRoles;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}");
            }
        }
    }

}