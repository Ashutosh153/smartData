using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class Cities
    {
        public int Id { get; set; }

        public string CityName { get; set; }

        public int StateId {  get; set; }
    }
}
