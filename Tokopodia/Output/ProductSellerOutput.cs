using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Models;

namespace Tokopodia.Output
{
    public class ProductSellerOutput
    {

        public ProductSellerOutput(Task<List<Product>> products)
        {
            Product = products;
        }

        public Task<List<Product>> Product { get; set; }
    }
}

