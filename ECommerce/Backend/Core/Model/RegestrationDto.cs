using Microsoft.AspNetCore.Http;

namespace App.Core.Model
{
    public class RegestrationDto
    {

      

        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Email { get; set; }


        public string Mobile { get; set; }

        public DateOnly DOB { get; set; }



        public int UserType_Id { get; set; }


        public string ProfileImage { get; set; }

        public string Address { get; set; }

        public int Zipcode { get; set; }



        public int Country_Id { get; set; }



        public int State_Id { get; set; }


    }
}
