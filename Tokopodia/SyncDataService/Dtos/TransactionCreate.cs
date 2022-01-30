using System.Collections.Generic;
namespace Tokopodia.SyncDataService.Dtos
{
  public class SellerCreateInput
  {
    public float amountSeller { get; set; }
    public int sellerId { get; set; }
  }
  public class TransactionCreate
  {
    public float amountBuyer { get; set; }
    public float amountCourier { get; set; }
    public int buyerId { get; set; }
    public IList<SellerCreateInput> sellers { get; set; }
  }
}