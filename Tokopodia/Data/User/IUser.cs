using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Output;

namespace Tokopodia.Data.User
{
  public interface IUser
  {
    Task<IdentityUser> Registration(IdentityUser user);
    Task<LoginOutput> Authenticate(IdentityUser user);
  }
}