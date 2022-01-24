using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Tokopodia.Models
{
  public class BuyerProfile
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public double latBuyer { get; set; }
    public double longBuyer { get; set; }

    public IdentityUser User { get; set; }

  }
}