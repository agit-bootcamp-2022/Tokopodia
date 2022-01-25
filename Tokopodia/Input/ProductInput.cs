namespace Tokopodia.Input
{
    public record ProductInput
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
