using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using Tokopodia.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tokopodia.Data.Wallets
{
  public class WalletDAL : IWallet
  {
    private readonly AppDbContext _db;

    public WalletDAL([Service] AppDbContext db)
    {
      _db = db;
    }

    public async Task<Wallet> GetByUserId(string UserId)
    {
      var result = await _db.Wallets.Where(w => w.UserId == UserId).FirstOrDefaultAsync();
      return result;
    }
    public async Task<Wallet> Insert(Wallet input)
    {
      var result = await _db.Wallets.AddAsync(input);
      await _db.SaveChangesAsync();
      return result.Entity;
    }
  }
}