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
        private readonly AppDbContext _context;

        public ProductBuyerOutput([Service] AppDbContext context)
        {
            _context = context;
        }

        public ProductBuyerOutput(IQueryable<Product> products, int sellerId)
        {
            var seller = _context.SellerProfiles.Where(o => o.Id == sellerId).FirstOrDefault();

            SellerName = seller.ShopName;

            Products = products;
        }

        public string SellerName { get; set; }
        public IQueryable<Product> Products { get; }
    }
}

