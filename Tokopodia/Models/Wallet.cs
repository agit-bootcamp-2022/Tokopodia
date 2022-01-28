using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tokopodia.Models
{
  public class Wallet
  {
    [Key]
    public int WalletId { get; set; }
    [Required]
    public string UserId { get; set; } //fk identity user
    [Required]
    public int UangTransId { get; set; } //fk dari wallet service -> create transaction

    [Required]
    public string Username { get; set; } //usernama for login wallet service

    public string Password { get; set; } //pass utk login ke wallet service

    public IdentityUser User { get; set; }

  }
}