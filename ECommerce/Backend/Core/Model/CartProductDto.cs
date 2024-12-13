using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class CartProductDto
    {
        
        
            public int CartId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public string ProductName { get; set; }
            public string Category { get; set; }
            public string Brand { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
        public int Stock { get; set; }
        
    }
}
