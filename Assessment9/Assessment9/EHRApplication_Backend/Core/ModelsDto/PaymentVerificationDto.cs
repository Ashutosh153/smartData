﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public class PaymentVerificationDto
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
    }

}