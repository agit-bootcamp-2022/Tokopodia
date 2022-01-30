namespace Tokopodia.SyncDataService.Dtos
{
  public class ShipmentInput
  {
    public int transactionId { get; set; }
    public string senderName { get; set; }
    public string senderContact { get; set; }
    public double senderLat { get; set; }
    public double senderLong { get; set; }
    public string receiverName { get; set; }
    public string receiverContact { get; set; }
    public double receiverLat { get; set; }
    public double receiverLong { get; set; }
    public double totalWeight { get; set; }
    public int shipmentTypeId { get; set; }
    public string token { get; set; }

  }
}