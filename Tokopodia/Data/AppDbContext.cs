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
    public DbSet<BuyerProfile> BuyerProfiles { get; set; }
    public DbSet<Cart> Carts { get; set; }
  }
}