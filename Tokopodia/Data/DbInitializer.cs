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

      if (!_context.Roles.Any(r => r.Name == "Buyer"))
      {
        await roleStore.CreateAsync(new IdentityRole { Name = "Buyer", NormalizedName = "BUYER" });
      }
      if (!_context.Roles.Any(r => r.Name == "Seller"))
      {
        await roleStore.CreateAsync(new IdentityRole { Name = "Seller", NormalizedName = "SELLER" });
      }
      if (!_context.Roles.Any(r => r.Name == "Courier"))
      {
        await roleStore.CreateAsync(new IdentityRole { Name = "Courier", NormalizedName = "COURIER" });
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
                new Product{SellerId = 1, ShopName= "MetropolisStore", Name = "Batman Action Figure", Category= "Toys" ,
                    Description="Batman action figure for collectible, DC Original" , Stock= 20,
                    Price=300000, Weight=100 ,Created= DateTime.Now},
                new Product{SellerId = 1, ShopName= "MetropolisStore", Name = "Superman Action Figure", Category= "Toys" ,
                    Description="Superman action figure for collectible, DC Original" , Stock= 10,
                    Price=350000, Weight=100 ,Created= DateTime.Now},
                new Product{SellerId = 1, ShopName= "MetropolisStore", Name = "Power Bank", Category= "Technology" ,
                    Description="Power bank for convinient phone charger" , Stock= 10,
                    Price=500000, Weight=200 ,Created= DateTime.Now},
                new Product{SellerId = 2, ShopName= "GothamStore", Name = "IronMan T-Shirt", Category= "Clothed" ,
                    Description="T-Shirt with arc reactor symbol" , Stock= 30,
                    Price=100000, Weight=20 ,Created= DateTime.Now},
                new Product{SellerId = 2, ShopName= "GothamStore", Name = "Doctor Strange T-Shirt", Category= "Clothed" ,
                    Description="T-Shirt with sactum sactorium symbol" , Stock= 30,
                    Price=100000, Weight=20 ,Created= DateTime.Now},
                new Product{SellerId = 3, ShopName= "AmazonianStore", Name = "Samsung S20", Category= "Technology" ,
                    Description="Samsung S20 Phone" , Stock= 7,
                    Price=3000000, Weight=120 ,Created= DateTime.Now},
      };

      foreach (var s in products)
      {
        context.Products.Add(s);
      }

      context.SaveChanges();
    }

    public static void Initialize(AppDbContext context)
    {
      context.Database.EnsureCreated();

      if (context.Carts.Any())
      {
        return;
      }


      var carts = new Cart[]
      {
                new Cart() { BuyerId = 1, ProductId = 1, SellerId = 1, Quantity = 1, Weight = 100, BillingSeller = 300000, LatSeller = -6.225407736894751, LongSeller = 106.94341773119231, LatBuyer = -6.206607748846436, LongBuyer = 106.94433053307974, ShippingId = 0, ShippingTypeId = 1, ShippingCost = 11000, Status = "OnTransaction"},
                new Cart() { BuyerId = 1, ProductId = 2, SellerId = 1, Quantity = 1, Weight = 100 ,BillingSeller = 350000, LatSeller = -6.225407736894751, LongSeller = 106.94341773119231, LatBuyer = -6.206607748846436, LongBuyer = 106.94433053307974, ShippingId = 0, ShippingTypeId = 1, ShippingCost = 11000, Status = "OnTransaction"},
                new Cart() { BuyerId = 1, ProductId = 4, SellerId = 2, Quantity = 1, Weight = 20 ,BillingSeller = 100000, LatSeller = -6.217151172929278, LongSeller = 106.92365729212868, LatBuyer = -6.206607748846436, LongBuyer = 106.94433053307974, ShippingId = 0, ShippingTypeId = 1, ShippingCost = 11000, Status = "OnTransaction"},
                new Cart() { BuyerId = 2, ProductId = 5, SellerId = 2, Quantity = 1, Weight = 20 ,BillingSeller = 100000, LatSeller = -6.217151172929278, LongSeller = 106.92365729212868, LatBuyer = -6.204530320309839, LongBuyer = 106.94925525069073, ShippingId = 0, ShippingTypeId = 2, ShippingCost = 11000, Status = "OnCart"},
                new Cart() { BuyerId = 2, ProductId = 6, SellerId = 3, Quantity = 1, Weight = 120 ,BillingSeller = 3000000, LatSeller = -6.217151172929278, LongSeller = 106.92365729212868, LatBuyer = -6.204530320309839, LongBuyer = 106.94925525069073, ShippingId = 0, ShippingTypeId = 2, ShippingCost = 11000, Status = "OnCart"}
      };

      foreach (var c in carts)
      {
        context.Carts.Add(c);
      }

      context.SaveChanges();
    }
  }
}