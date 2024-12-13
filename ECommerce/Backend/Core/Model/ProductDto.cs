using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class ProductDto
    {
      

        public string ProductName { get; set; }


        public int ProductId { get; set; }

        public string ProductImage { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }


        public float SellingPrice { get; set; }


        public float PurchasePrice { get; set; }

        public DateOnly PurchaseDate { get; set; }


        public int Stock { get; set; } 
        public int IsCreatedCy {  get; set; }
    }
}
