using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public class GetAllAppointmentDto
    {
        public int id { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string ChiefComplaint { get; set; }
         public string AppointmentStatus { get; set; } 
        public int Fee { get; set; }

        public string PatientFullName { get; set; }

        public string PractionerFullName {  get; set; }

        public string profile { get; set; }

        public string practionerSpecialisation {  get; set; }
    }
}
