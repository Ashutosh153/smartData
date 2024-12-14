using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public class GetPractionerForAppointmentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Profile { get; set; }
        public string Qualification { get; set; }
        public string Registration_Number { get; set; }
        public string Visiting_Charge { get; set; }
        public string SpecialisationType { get; set; }
    }
}
