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

  }
}