using System;

namespace Tokopodia.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public DateTime Created { get; set; }
    }
}
