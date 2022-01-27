using HotChocolate;

namespace Tokopodia.Input
{
  public class LoginInput
  {
    [GraphQLNonNullType]
    public string Username { get; set; }
    [GraphQLNonNullType]
    public string Password { get; set; }
  }
}