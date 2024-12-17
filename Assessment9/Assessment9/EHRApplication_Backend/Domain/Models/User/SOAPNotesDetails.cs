using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class SOAPNotesDetails
    {
        public int id {  get; set; }
        public int AppointmnetID {  get; set; }
        public string Subjective {  get; set; }
        public string Objective { get; set; }
        public string Assessment { get; set; }

        public  string Plan {  get; set; }

    }
}
