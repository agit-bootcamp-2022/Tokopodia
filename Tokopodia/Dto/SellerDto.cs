using System;

namespace Tokopodia.Dto
{
    public partial class SellerDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }

        public string ShopName { get; set; }
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
