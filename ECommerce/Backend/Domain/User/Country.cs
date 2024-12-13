using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class Country
    {
        [Key]
        public int Id { get; set; }


        public string Name { get; set; }

        public string ShortName {  get; set; }

        public int PhoneCode {  get; set; }

    }
}
