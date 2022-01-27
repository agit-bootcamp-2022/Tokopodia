using HotChocolate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tokopodia.Helpers;
using Tokopodia.Models;

namespace Tokopodia.Data.SellerProfiles
{
    public class SellerProfileDAL : ISellerProfile
    {
        private UserManager<IdentityUser> _userManager;

        private RoleManager<IdentityRole> _roleManager;
        private AppSettings _appSettings;
        private readonly AppDbContext _db;

        public SellerProfileDAL([Service] UserManager<IdentityUser> userManager,
                       [Service] RoleManager<IdentityRole> roleManager,
                       [Service] IOptions<AppSettings> appSettings,
                       [Service] AppDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
            _db = db;
        }

        public async Task<SellerProfile> GetByUserId(string userId)
        {
            var result = await _db.SellerProfiles.Where(b => b.UserId == userId).Include(b => b.User).FirstOrDefaultAsync();
            return result; ;
        }

        public Task<SellerProfile> GetProfileById(int profileId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SellerProfile> Insert(SellerProfile profile)
        {
            var result = await _db.SellerProfiles.AddAsync(profile);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<SellerProfile> Update(SellerProfile profile)
        {
            var result = _db.SellerProfiles.Update(profile);
            await _db.SaveChangesAsync();
            return result.Entity;
        }
    }
}
