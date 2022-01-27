using HotChocolate;

namespace Tokopodia.Input
{
  public class BuyerPositionInput
  {
    [GraphQLNonNullType]
    public double LatBuyer { get; set; }
    [GraphQLNonNullType]
    public double LongBuyer { get; set; }
  }
}