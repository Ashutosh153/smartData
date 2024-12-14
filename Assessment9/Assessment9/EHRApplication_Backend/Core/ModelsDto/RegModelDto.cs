using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public class RegModelDto
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        //public int CountryId { get; set; }
        public int PinCode { get; set; }
        public string Profile { get; set; }
        public string Mobile { get; set; }
          public int UserType_ID { get; set; }


        public string? Qualification { get; set; }
        public int? Specialisation_ID { get; set; }
        public string? Registration_Number { get; set; }
        public int? Visiting_Charge { get; set; }

    }
}
