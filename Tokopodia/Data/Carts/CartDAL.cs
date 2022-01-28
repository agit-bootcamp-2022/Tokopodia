using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using Tokopodia.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tokopodia.Data.Carts
{
  public class CartDAL : ICart
  {
    private readonly AppDbContext _db;

    public CartDAL([Service] AppDbContext db)
    {
      _db = db;
    }
    public async Task<IEnumerable<Cart>> GetAllByStatusOnCart()
    {
      var result = await _db.Carts.Include(c => c.Product).Include(c => c.Buyer).Include(c => c.Seller).Where(c => c.Status == "OnCart").ToListAsync();
      return result;
    }

    public async Task<Cart> Update(Cart input)
    {
      var result = _db.Carts.Update(input);
      await _db.SaveChangesAsync();
      return result.Entity;
    }
  }
}