using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tokopodia.Models
{
    public partial class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SellerId { get; set; }
        [Required]
        public string ShopName { get; set; }
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public IEnumerable<Cart> Cart { get; set; }
    }
}
