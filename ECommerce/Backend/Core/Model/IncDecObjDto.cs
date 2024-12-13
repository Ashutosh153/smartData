using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class IncDecObjDto
    {
        public int cartId { get; set; }
        public int productId { get; set; }
    }
}
