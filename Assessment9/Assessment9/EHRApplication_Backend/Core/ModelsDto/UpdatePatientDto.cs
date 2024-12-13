using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public  class UpdatePatientDto
    {
        public int Id {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public int PinCode { get; set; }
        public string Profile {  get; set; }
    }
}
