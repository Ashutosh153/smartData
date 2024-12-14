using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public class AddAppointmentDto
    {
        public int PatientId { get; set; } 
        public int ProviderId { get; set; } 
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string ChiefComplaint { get; set; }
       // public string AppointmentStatus { get; set; } 
        public int Fee { get; set; }
    }
}
