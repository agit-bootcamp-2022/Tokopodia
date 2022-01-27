using System.Threading.Tasks;
using Tokopodia.Models;

namespace Tokopodia.Data.SellerProfiles
{
    public interface ISellerProfile
    {
        Task<SellerProfile> Insert(SellerProfile profile);
        Task<SellerProfile> GetProfileById(int profileId);
        Task<SellerProfile> GetByUserId(string userId);
        Task<SellerProfile> Update(SellerProfile profile);
    }
}
