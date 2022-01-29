using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using Tokopodia.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tokopodia.Data.Transactions
{
  public class TransactionDAL : ITransaction
  {
    private readonly AppDbContext _db;

    public TransactionDAL([Service] AppDbContext db)
    {
      _db = db;
    }

    public async Task<Transaction> Insert(Transaction input)
    {
      var result = await _db.Transactions.AddAsync(input);
      await _db.SaveChangesAsync();
      return result.Entity;
    }

    public async Task<Transaction> Update(Transaction input)
    {
      var result = _db.Transactions.Update(input);
      await _db.SaveChangesAsync();
      return result.Entity;
    }

    public async Task<Transaction> GetById(int transactionId)
    {
      var result = await _db.Transactions
                          .Include(t => t.Carts)
                            .ThenInclude(t => t.Buyer)
                          .Include(t => t.Carts)
                            .ThenInclude(t => t.Seller)
                          .Include(t => t.Carts)
                            .ThenInclude(t => t.Product)
                          .Where(t => t.TransactionId == transactionId).FirstOrDefaultAsync();
      return result;
    }

    public async Task<IEnumerable<Transaction>> GetByProfileId(int profileId, string profileType)
    {
      List<Transaction> result = new List<Transaction>();
      if (profileType == "Seller")
      {
        result = await _db.Transactions
                         .Include(t => t.Carts)
                           .ThenInclude(t => t.Buyer)
                         .Include(t => t.Carts.Where(c => c.Seller.Id == profileId))
                           .ThenInclude(t => t.Seller)
                         .Include(t => t.Carts)
                           .ThenInclude(t => t.Product)
                          .ToListAsync();

      }
      else if (profileType == "Buyer")
      {
        result = await _db.Transactions
                                 .Include(t => t.Carts.Where(c => c.Seller.Id == profileId))
                                   .ThenInclude(t => t.Buyer)
                                 .Include(t => t.Carts)
                                   .ThenInclude(t => t.Seller)
                                 .Include(t => t.Carts)
                                   .ThenInclude(t => t.Product)
                                  .ToListAsync();
      }
      return result;
    }

  }
}