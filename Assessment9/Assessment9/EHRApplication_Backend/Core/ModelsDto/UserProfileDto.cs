using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.ModelsDto
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int PinCode { get; set; }
        public string Profile { get; set; }
        public string Mobile { get; set; }

        public int UserType_ID { get; set; }
        public string? Qualification { get; set; }
        public int? Specialisation_ID { get; set; }
        public string? Registration_Number { get; set; }
        public float? Visiting_Charge { get; set; }

        public string CityName {  get; set; }

        public string StateName {  get; set; }
        public string Specilisation {  get; set; }

    }
}
