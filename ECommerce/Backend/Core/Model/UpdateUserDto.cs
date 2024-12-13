using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string userName{  get; set; }


        public string LastName { get; set; }

        public string Profile {  get; set; }


        public string Email { get; set; }


        public string Mobile { get; set; }

        public DateOnly DOB { get; set; }


        public string Address { get; set; }

        public int Zipcode { get; set; }


        public int Country_Id { get; set; }

        public int State_Id { get; set; }
    }
}
