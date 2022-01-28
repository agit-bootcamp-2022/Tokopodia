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
        public double Weight { get; set; }
        public double BillingSeller { get; set; }
        public double LatSeller { get; set; }
        public double LongSeller { get; set; }
        public double LatBuyer { get; set; }
        public double LongBuyer { get; set; }
        public int ShippingId { get; set; }
        public int ShippingTypeId { get; set; }
        public double ShippingCost { get; set; }
        public int TransactionId { get; set; }
        public string Status { get; set; }

        public Product Product { get; set; }
        public BuyerProfile Buyer { get; set; }
        public SellerProfile Seller { get; set; }
    }
}
