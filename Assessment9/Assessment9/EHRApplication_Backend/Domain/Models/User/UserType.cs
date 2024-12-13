using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class UserType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
