using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tokopodia.Data;
using Tokopodia.Models;

namespace Tokopodia.Output
{
    public class ProductBuyerOutput
    {

        public ProductBuyerOutput(Task<List<Product>> products)
        {
            Products = products;
        }

        public Task<List<Product>> Products { get; }
    }
}

