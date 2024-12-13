using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public  class UpdateProviderDto
    {
        public int Id { get; set; }
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


        public string? Qualification { get; set; }
        public int? Specialisation_ID { get; set; }
        public string? Registration_Number { get; set; }
        public float? Visiting_Charge { get; set; }
    }
}
