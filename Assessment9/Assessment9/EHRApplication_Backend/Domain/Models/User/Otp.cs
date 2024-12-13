using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class Otp
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string otp { get; set; }
    }
}
