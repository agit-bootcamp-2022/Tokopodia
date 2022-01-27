namespace Tokopodia.SyncDataService.Dtos
{
  public class Data
  {
    public int shipmentId { get; set; }
    public string statusOrder { get; set; }
  }
  public class ShipmentOutput
  {
    public string status { get; set; }
    public Data data { get; set; }
  }
}