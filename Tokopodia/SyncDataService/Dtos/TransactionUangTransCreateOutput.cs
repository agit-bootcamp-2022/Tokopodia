using System.Collections.Generic;
namespace Tokopodia.SyncDataService.Dtos
{

  public class TransactionCreateOutput
  {
    public string message { get; set; }
    public int transactionId { get; set; }
  }

  public class TransactionUangTransCreateOutput
  {
    public TransactionCreateOutput createTransaction { get; set; }
  }
}