using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Data.BuyerProfiles
{
  public interface IBuyerProfile
  {
    Task<BuyerProfile> Insert(BuyerProfile profile);
    Task<BuyerProfile> GetProfileById(int profileId);
  }
}