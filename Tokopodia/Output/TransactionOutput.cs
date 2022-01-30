using System;
using System.Collections.Generic;
using Tokopodia.Models;
namespace Tokopodia.Output
{
  public class CartOutput
  {
    public int Id { get; set; }
    public string BuyerName { get; set; }

    public string SellerName { get; set; }
    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public double Weight { get; set; }

    public double LatSeller { get; set; }

    public double LongSeller { get; set; }

    public double LatBuyer { get; set; }

    public double LongBuyer { get; set; }
    public int ShippingId { get; set; }
    public int ShippingTypeId { get; set; }
    public double ShippingCost { get; set; }
  }
  public class TransactionOutput
  {
    public int TransactionId { get; set; }

    public IEnumerable<CartOutput> CartsOutput { get; set; }

    public string Address { get; set; }


    public double SumBillingSeller { get; set; }


    public double SumShippingCost { get; set; }


    public double TotalBilling { get; set; }


    public TransactionStatus status { get; set; }
    public DateTime CreatedAt { get; set; }

    public int WalletTransactionId { get; set; } //fk dari wallet service
  }
}
