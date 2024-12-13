using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string FirstName  {  get; set; }
           public string  LastName { get; set; }
        public DateOnly  DateOfBirth    {  get; set; }
           public string  UserName {  get; set; }
        public string Password { get; set; }
           public  string  Gender  {  get; set; }
            public string Email {  get; set; }
          public    string  BloodGroup {  get; set; }
          public  string  Address {  get; set; }
           public   string City {  get; set; }
           public   int StateId {  get; set; }
        public int CountryId { get; set; }
          public     int PinCode {  get; set; }
        public string Profile {  get; set; }

        [ForeignKey("UserType")]
        public   int UserType_ID {  get; set; }
          public    string? Qualification {  get; set; }
        [ForeignKey("Specialisation")]
        public   int? Specialisation_ID {  get; set; }
         public   string? Registration_Number {  get; set; }
         public   float? Visiting_Charge {  get; set; }

        public UserType UserType { get; set; }
        public Specialisation Specialisation { get; set; }
    }
}
