using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tokopodia.Models
{
  public enum TransactionStatus
  {
    Paid,
    Completed
  }
  public class Transaction
  {
    [Key]
    public int TransactionId { get; set; }

    public IEnumerable<Cart> Carts { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public double SumBillingSeller { get; set; }

    [Required]
    public double SumShippingCost { get; set; }

    [Required]
    public double TotalBilling { get; set; }

    [Required]
    public TransactionStatus status { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public string Token { get; set; }

    public int WalletTransactionId { get; set; } //fk dari wallet service

  }
}