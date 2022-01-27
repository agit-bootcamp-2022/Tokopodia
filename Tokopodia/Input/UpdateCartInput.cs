namespace Tokopodia.Input
{
    public class UpdateCartInput
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public double LatBuyer { get; set; }
        public double LongBuyer { get; set; }
        public int ShippingTypeId { get; set; }
    }
}
