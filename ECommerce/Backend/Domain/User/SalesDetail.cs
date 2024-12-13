using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class SalesDetail
    {
       
        public int Id { get; set; }

        
        public int InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public SalesMaster SalesMaster { get; set; }

      
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required, MaxLength(10)]
        public string ProductCode { get; set; }

       
        public int SaleQty { get; set; }

        public float SellingPrice { get; set; }
    }
}
