using System.Threading.Tasks;
using Tokopodia.Input;
using Tokopodia.Output;

namespace Tokopodia.SyncDataService.Http
{
    public interface IUangTransDataClient
    {
        Task<BalanceOutput> GetSaldoUser();
        Task SendWalletToUangTrans(WalletForHttpInput input);
    }
}
