using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Tokopodia.Models
{
    public class SellerProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string ShopName { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public double LatSeller { get; set; }
        public double LongSeller { get; set; }

        public IdentityUser User { get; set; }
    }
}
