using Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class SalesMasterDto
    {

        public int UserId { get; set; }

        public float Subtotal { get; set; }

        public string DeliveryAddress { get; set; }

        public int DeliveryZipcode { get; set; }

        public int DeliveryState { get; set; }

        public int DeliveryCountry { get; set; }



    }
}
