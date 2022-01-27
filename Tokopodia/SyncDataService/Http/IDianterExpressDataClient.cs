using System.Threading.Tasks;
using Tokopodia.SyncDataService.Dtos;

namespace Tokopodia.SyncDataService.Http
{
  public interface IDianterExpressDataClient
  {
    Task<ShipmentOutput> CreateShipment(ShipmentInput input);
  }
}
