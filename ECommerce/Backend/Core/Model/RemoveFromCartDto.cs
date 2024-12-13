using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public  class RemoveFromCartDto
    {
        public int userId { get; set; }
        public int productId {  get; set; }
    }
}
