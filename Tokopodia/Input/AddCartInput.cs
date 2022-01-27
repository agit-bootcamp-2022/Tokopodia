namespace Tokopodia.Input
{
    public record AddCartInput
    (
        int ProductId,
        int Quantity,
        double LatBuyer,
        double LongBuyer,
        string ShippingType
    );
}
