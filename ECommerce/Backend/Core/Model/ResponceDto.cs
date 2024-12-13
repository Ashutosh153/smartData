using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class ResponceDto
    {
        public int statusCode { get; set; } = 200;
        public string message { get; set; } = "";
        public object data { get; set; } = null;
        public bool isSuccess { get; set; } = false;
    }
}
