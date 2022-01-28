using System;
using System.Collections.Generic;
using Tokopodia.Models;
namespace Tokopodia.Output
{
  public class TransactionOutput
  {
    public int TransactionId { get; set; }

    public IEnumerable<Cart> Carts { get; set; }

    public string Address { get; set; }


    public double SumBillingSeller { get; set; }


    public double SumShippingCost { get; set; }


    public double TotalBilling { get; set; }


    public TransactionStatus status { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Token { get; set; }

    public int WalletTransactionId { get; set; } //fk dari wallet service
  }
}
