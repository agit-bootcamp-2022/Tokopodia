using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tokopodia.Models;

namespace Tokopodia.Data
{
  public class AppDbContext : IdentityDbContext
  {
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<BuyerProfile> BuyerProfiles { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<SellerProfile> SellerProfiles { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
  }
}