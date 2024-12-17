using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public class GetUserForAppointmentDto
    {
        public string Profile { get; set; }
        public string gender { get; set; }
        public DateTime dob { get; set; }
        public string PatientName { get; set; }
        public string ChiefComplaint { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
    }
}
