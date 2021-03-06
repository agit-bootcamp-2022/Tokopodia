using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Input;
using Tokopodia.Output;

namespace Tokopodia.Data.Users
{
  public interface IUser
  {
    Task<IdentityUser> Registration(IdentityUser user);
    Task<LoginOutput> Authenticate(LoginInput user, string userType);
    Task<IdentityUser> UpdateRole(IdentityUser user, string role);

    Task<IdentityUser> GetById(string UserId);
  }
}