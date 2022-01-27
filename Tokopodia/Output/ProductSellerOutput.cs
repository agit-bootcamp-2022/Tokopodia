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

        private readonly AppDbContext _context;

        public ProductSellerOutput([Service] AppDbContext context)
        {
            _context = context;
        }

        public ProductSellerOutput(Task<List<Product>> products, int sellerId)
        {
            var seller = _context.SellerProfiles.Where(o => o.Id == sellerId).FirstOrDefault();

            SellerName = seller.ShopName;

            Product = products;
        }

        public string SellerName { get; set; }
        public Task<List<Product>> Product { get; set; }
    }
}

