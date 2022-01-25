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
    public string Username { get; set; } //usernama for login wallet service

    public IdentityUser User { get; set; }

  }
}