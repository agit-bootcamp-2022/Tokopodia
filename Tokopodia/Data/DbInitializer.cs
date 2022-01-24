using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Tokopodia.Models;

namespace Tokopodia.Data
{
    public class DbInitializer
    {
        private AppDbContext _context;

        public DbInitializer(AppDbContext context)
        {
            _context = context;
        }

        public async Task Initialize(UserManager<IdentityUser> userManager)
        {
            _context.Database.EnsureCreated();
            var roleStore = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == "buyer"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "buyer", NormalizedName = "BUYER" });
            }
            if (!_context.Roles.Any(r => r.Name == "seller"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "seller", NormalizedName = "SELLER" });
            }
            if (!_context.Roles.Any(r => r.Name == "courier"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "courier", NormalizedName = "COURIER" });
            }
            if (!_context.Users.Any(u => u.UserName == "courier1"))
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "courier1",
                    Email = "courier@antarexpress.com"
                };
                IdentityResult result = await userManager.CreateAsync(user, "Courier1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "courier");
                }
            }
            await _context.SaveChangesAsync();

        }

        public static void Initializer(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;
            }

            var students = new Product[]
            {
                
            };

            foreach (var s in students)
            {
                context.Products.Add(s);
            }

            context.SaveChanges();
        }
    }
}