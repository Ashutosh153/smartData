using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Email { get; set; }


        public string Username { get; set; }


        public string Password { get; set; }

 
        public string Mobile { get; set; }

        public DateOnly DOB { get; set; }


        [ForeignKey("UserType")]
        public int UserType_Id { get; set; }//userType foreign key
        public UserType UserType { get; set; }

        public string ProfileImage { get; set; } //optional while regestration

        public string Address { get; set; } //optional while regestration

        public int Zipcode { get; set; }


        [ForeignKey("Country")]
        public int Country_Id { get; set; }  // country foreign key
        public Country Country { get; set; }

        [ForeignKey("State")]
        public int State_Id { get; set; } //state foreign 
        public State State { get; set; }

       


    }
}
