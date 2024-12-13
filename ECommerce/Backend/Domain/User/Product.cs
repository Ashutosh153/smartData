using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

       
        public string ProductCode { get; set; }

        public string ProductImage { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

 
        public float SellingPrice { get; set; }


        public float PurchasePrice { get; set; }

        public DateOnly PurchaseDate { get; set; }

    
        public int Stock { get; set; }

        public int IsDeleted { get; set; }

        public int IsCreatedCy { get; set; }
    }
}
