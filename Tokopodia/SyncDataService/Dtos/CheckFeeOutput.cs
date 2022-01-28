namespace Tokopodia.SyncDataService.Dtos
{
    public class Data1
    {
        public double fee { get; set; }
    }
    public class CheckFeeOutput
    {
        public string status { get; set; }
        public Data1 data { get; set; }
    }
}
