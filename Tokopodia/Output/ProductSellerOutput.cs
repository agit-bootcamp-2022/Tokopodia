using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tokopodia.Models;

namespace Tokopodia.Output
{
    public class ProductSellerOutput
    {
        //var seller = context.Sellers.Where(o => o.SellerId == sellerId).FirstOrDefault();

        //SellerName = seller.shopname;

        public ProductSellerOutput(Task<List<Product>> product)
        {
            Product = product;
        }

        public string SellerName { get; set; }
        public Task<List<Product>> Product { get; set; }
    }
}

