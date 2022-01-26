using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.Options;

namespace Tokopodia.GraphQL.Queries
{
  [ExtendObjectType(Name = "Query")]
  [System.Obsolete]
  public class BuyerProfileQuery
  {

    // TODO: mbuat utk get buyer profile
    public string GetBuyerProfile()
    {
      return "Hello buyer profile!";
    }



  }
}