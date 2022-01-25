namespace Tokopodia.Input
{
  public record ProductBuyerInput
   (
       int? Id,
       string Name,
#nullable enable
       string? Category,
       double? MaxPrice,
       double? MinPrice
   );
}
