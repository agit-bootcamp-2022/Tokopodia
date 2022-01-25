using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tokopodia.Models
{
  public enum Status
  {
    OnProgress,
    Finished
  }
  public class Transaction
  {
    // TODO: cek comment dibawah ini.
    [Key]
    public int TransactionId { get; set; }

    // sesuaikan dengan cart id yang akan dibuat
    // public ICollection<Cart> Carts { get; set; } 

    // menunggu dari shipping service
    // public int ShippingId { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public double SumBillingSeller { get; set; }

    [Required]
    public double SumBillingAntar { get; set; }

    [Required]
    public double TotalBilling { get; set; }

    [Required]
    public Status status { get; set; }

    public string Token { get; set; }

  }
}