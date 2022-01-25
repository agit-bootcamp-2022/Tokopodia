namespace Tokopodia.Input
{
    public record ProductSellerInput
     (
         int? Id,
         string Name,
         string Category,
         string Description,
         int Stock,
         double Price,
         double Weight
     );
}
