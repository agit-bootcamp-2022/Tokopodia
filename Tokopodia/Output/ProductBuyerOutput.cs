using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tokopodia.Models;

namespace Tokopodia.Output
{
    public class ProductBuyerOutput
    {
        //var seller = context.Sellers.Where(o => o.SellerId == sellerId).FirstOrDefault();

        //SellerName = seller.shopname;

        public ProductBuyerOutput(IQueryable<Product> products)
        {
            Products = products;
        }

        public string SellerName { get; set; }
        public IQueryable<Product> Products { get; }
    }
}

