using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Data.Wallets
{
  public interface IWallet
  {
    Task<Wallet> GetByUserId(string UserId);
    Task<Wallet> Insert(Wallet input);
  }
}