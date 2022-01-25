using System;

namespace Tokopodia.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public int Quantity { get; set; }
        public float BillingSeller { get; set; }
        public double LatSeller { get; set; }
        public double LongSeller { get; set; }
        public double LatBuyer { get; set; }
        public double LongBuyer { get; set; }
        public string ShippingType { get; set; }
        public float ShippingCost { get; set; }
        public string Status { get; set; }
    }
}
