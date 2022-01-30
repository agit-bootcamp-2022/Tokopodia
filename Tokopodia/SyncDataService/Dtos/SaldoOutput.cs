using System.Collections.Generic;
namespace Tokopodia.SyncDataService.Dtos
{
  public class WalletCustomer
  {
    public int id { get; set; }
    public float balance { get; set; }
    public string createdDate { get; set; }
    public int customerId { get; set; }
  }
  public class WalletData
  {
    public IList<WalletCustomer> walletByCustomerIdAsync { get; set; }
  }
  public class SaldoOutput
  {
    public WalletData data { get; set; }
  }

}
