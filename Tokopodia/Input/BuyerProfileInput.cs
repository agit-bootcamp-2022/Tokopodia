using HotChocolate;

namespace Tokopodia.Input
{
  public class BuyerProfileInput
  {
    [GraphQLNonNullType]
    public string FirstName { get; set; }
    [GraphQLNonNullType]
    public string LastName { get; set; }

    [GraphQLNonNullType]
    public string Address { get; set; }
    [GraphQLNonNullType]
    public double LatBuyer { get; set; }
    [GraphQLNonNullType]
    public double LongBuyer { get; set; }
  }
}