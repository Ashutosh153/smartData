using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public  class SalesMaster
    {
        public int Id { get; set; }

        public string InvoiceId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime InvoiceDate { get; set; }

       
        public float Subtotal { get; set; }

        
        public string DeliveryAddress { get; set; }

        
        public int DeliveryZipcode { get; set; }

      
        public int DeliveryState { get; set; }

        [ForeignKey("DeliveryState")]
        public State State { get; set; }

        
        public int DeliveryCountry { get; set; }

        [ForeignKey("DeliveryCountry")]
        public Country Country { get; set; }
    }
}
