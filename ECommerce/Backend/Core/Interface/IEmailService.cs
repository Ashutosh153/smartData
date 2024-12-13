﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.core.interfaces
{
    public  interface IEmailService
    {
        Task<bool> SendEmailAsync(string subject, string to, string body);

        

    }
}