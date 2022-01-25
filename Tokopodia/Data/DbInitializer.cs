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

            var products = new Product[]
            {
                new Product{SellerId = 1, Name = "Batman Action Figure", Category= "Toys" , 
                    Description="Batman action figure for collectible, DC Original" , Stock= 20, 
                    Price=300000, Weight=100 ,Created= DateTime.Now},
                new Product{SellerId = 1, Name = "Superman Action Figure", Category= "Toys" ,
                    Description="Superman action figure for collectible, DC Original" , Stock= 10,
                    Price=350000, Weight=100 ,Created= DateTime.Now},
                new Product{SellerId = 1, Name = "Power Bank", Category= "Technology" ,
                    Description="Power bank for convinient phone charger" , Stock= 10,
                    Price=500000, Weight=200 ,Created= DateTime.Now},
                new Product{SellerId = 2, Name = "IronMan T-Shirt", Category= "Clothed" ,
                    Description="T-Shirt with arc reactor symbol" , Stock= 30,
                    Price=100000, Weight=20 ,Created= DateTime.Now},
                new Product{SellerId = 2, Name = "Doctor Strange T-Shirt", Category= "Clothed" ,
                    Description="T-Shirt with sactum sactorium symbol" , Stock= 30,
                    Price=100000, Weight=20 ,Created= DateTime.Now},
                new Product{SellerId = 3, Name = "Samsung S20", Category= "Technology" ,
                    Description="Samsung S20 Phone" , Stock= 7,
                    Price=3000000, Weight=120 ,Created= DateTime.Now},
            };

            foreach (var s in products)
            {
                context.Products.Add(s);
            }

            context.SaveChanges();
        }
    }
}