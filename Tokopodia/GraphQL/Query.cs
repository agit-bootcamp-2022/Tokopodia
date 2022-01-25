using System;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.Options;

namespace Tokopodia.Graphql
{
    [ExtendObjectType(Name = "Query")]
    [Obsolete]
  public class Query
  {

    public string GetHelloWorld()
    {
      return "Hello World!";
    }



  }
}