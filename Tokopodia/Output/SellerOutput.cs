using System;

namespace Tokopodia.Output
{
    public class SellerOutput
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }

        public double latSeller { get; set; }
        public double longSeller { get; set; }

        public UserOutput userOutput { get; set; }
    }
}
