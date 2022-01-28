using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using Tokopodia.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tokopodia.Data.Products
{
  public class ProductDAL : IProduct
  {
    private readonly AppDbContext _db;

    public ProductDAL([Service] AppDbContext db)
    {
      _db = db;
    }
    public async Task<Product> Update(Product input)
    {
      var result = _db.Products.Update(input);
      await _db.SaveChangesAsync();
      return result.Entity;
    }
  }
}