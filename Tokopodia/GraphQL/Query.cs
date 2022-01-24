using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.Extensions.Options;

namespace Tokopodia.GraphQL
{
  public class Query
  {

    public string GetHelloWorld()
    {
      return "Hello World!";
    }



  }
}