using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Data.Transactions
{
  public interface ITransaction
  {
    Task<Transaction> Insert(Transaction input);
  }
}