namespace Tokopodia.Input
{
    public record ProductBuyerInput
     (
         int? Id,
         string Name,
         string? Category,
         double? MaxPrice,
         double? MinPrice
     );
}
