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

            //Product
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

            foreach (var p in products)
            {
                context.Products.Add(p);
            }

            context.SaveChanges();

            //Carts
            if (context.Carts.Any())
            {
                return;
            }

            var carts = new Cart[]
            {
                new Cart() { BuyerId = 1, ProductId = 1, SellerId = 1, Quantity = 1, BillingSeller = 300000, LatSeller = -6.225407736894751, LongSeller = 106.94341773119231, LatBuyer = -6.206607748846436, LongBuyer = 106.94433053307974, ShippingType = "Reguler", ShippingCost = 11000, Status = "OnProcess"},
                new Cart() { BuyerId = 1, ProductId = 2, SellerId = 1, Quantity = 1, BillingSeller = 350000, LatSeller = -6.225407736894751, LongSeller = 106.94341773119231, LatBuyer = -6.206607748846436, LongBuyer = 106.94433053307974, ShippingType = "Reguler", ShippingCost = 11000, Status = "OnProcess"},
                new Cart() { BuyerId = 1, ProductId = 4, SellerId = 2, Quantity = 1, BillingSeller = 100000, LatSeller = -6.217151172929278, LongSeller = 106.92365729212868, LatBuyer = -6.206607748846436, LongBuyer = 106.94433053307974, ShippingType = "Reguler", ShippingCost = 11000, Status = "OnProcess"},
                new Cart() { BuyerId = 2, ProductId = 5, SellerId = 2, Quantity = 1, BillingSeller = 100000, LatSeller = -6.217151172929278, LongSeller = 106.92365729212868, LatBuyer = -6.204530320309839, LongBuyer = 106.94925525069073, ShippingType = "Reguler", ShippingCost = 11000, Status = "OnCart"},
                new Cart() { BuyerId = 2, ProductId = 6, SellerId = 2, Quantity = 1, BillingSeller = 3000000, LatSeller = -6.217151172929278, LongSeller = 106.92365729212868, LatBuyer = -6.204530320309839, LongBuyer = 106.94925525069073, ShippingType = "Reguler", ShippingCost = 11000, Status = "OnCart"}
            };

            foreach (var c in carts)
            {
                context.Carts.Add(c);
            }

            context.SaveChanges();

            //Seller
            if (context.SellerProfiles.Any())
            {
                return;
            }

            var sellers = new SellerProfile[]
            {
                new SellerProfile() { UserId = "1", Username = "ClarkKent", ShopName = "KryptonStore", Address = "Metropolis 202 Street", CreatedAt = DateTime.Now, LatSeller = -6.225407736894751, LongSeller = 106.94341773119231},
                new SellerProfile() { UserId = "2", Username = "BruceWayne", ShopName = "WayneStore", Address = "Gotham 101 Street", CreatedAt = DateTime.Now, LatSeller = -6.217151172929278, LongSeller = 106.92365729212868},
                new SellerProfile() { UserId = "3", Username = "DianaPrince", ShopName = "AmazonianStore", Address = "Amazon 32 Street", CreatedAt = DateTime.Now, LatSeller = -6.562351172923248, LongSeller = 106.933457513245868}
            };

            foreach (var s in sellers)
            {
                context.SellerProfiles.Add(s);
            }

            context.SaveChanges();

            //Buyer
            if (context.BuyerProfiles.Any())
            {
                return;
            }

            var buyers = new BuyerProfile[]
            {
                new BuyerProfile() { UserId = "4", FirstName = "Steve", LastName = "Rogers", CreatedAt = DateTime.Now, latBuyer = -6.206607748846436, longBuyer = 106.94433053307974},
                new BuyerProfile() { UserId = "5", FirstName = "Tony", LastName = "Stark", CreatedAt = DateTime.Now, latBuyer = -6.204530320309839, longBuyer = 106.94925525069073},
                new BuyerProfile() { UserId = "6", FirstName = "Thor", LastName = "Odinson", CreatedAt = DateTime.Now, latBuyer = -6.312340425302839, longBuyer = 106.93424525069073}
            };

            foreach (var b in buyers)
            {
                context.BuyerProfiles.Add(b);
            }

            context.SaveChanges();
        }
  }
}