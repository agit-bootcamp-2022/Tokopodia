using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Data.Carts
{
  public interface ICart
  {
    Task<IEnumerable<Cart>> GetAllByStatusOnCart();
  }
}