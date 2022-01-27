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
    // TODO: getallbystatusoncart harus include product => update model lagi
    public async Task<IEnumerable<Cart>> GetAllByStatusOnCart()
    {
      var result = await _db.Carts.Where(c => c.Status == "OnCart").ToListAsync();
      return result;
    }
  }
}