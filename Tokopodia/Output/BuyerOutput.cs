using System;
namespace Tokopodia.Output
{
  public class BuyerOutput
  {
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime CreatedAt { get; set; }

    public double latBuyer { get; set; }
    public double longBuyer { get; set; }

    public UserOutput userOutput { get; set; }
  }
}
