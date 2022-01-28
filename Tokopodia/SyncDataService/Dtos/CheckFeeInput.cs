namespace Tokopodia.SyncDataService.Dtos
{
    public class CheckFeeInput
    {
        public double senderLat { get; set; }
        public double senderLong { get; set; }
        public double receiverLat { get; set; }
        public double receiverLong { get; set; }
        public double weight { get; set; }
        public int shipmentTypeId { get; set; }
    }
}
