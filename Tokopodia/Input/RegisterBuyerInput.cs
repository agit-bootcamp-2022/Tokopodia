using HotChocolate;

namespace Tokopodia.Input
{
  public class RegisterBuyerInput
  {
    [GraphQLNonNullType]
    public string Username { get; set; }
    [GraphQLNonNullType]
    public string Email { get; set; }
    [GraphQLNonNullType]
    public string Password { get; set; }

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
