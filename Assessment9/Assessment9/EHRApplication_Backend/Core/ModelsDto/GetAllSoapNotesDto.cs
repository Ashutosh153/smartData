using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public class GetAllSoapNotesDto
    {
        public int id { get; set; }
        public int AppointmnetID { get; set; }
        public string Subjective { get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }
        public DateTime appointmentDate { get; set; }

        public string Plan { get; set; }
        public string ChiefComplaint { get; set; }

        public string patientName { get; set; }
        public string providerName { get; set; }



    }
}
