﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class State
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }


        [ForeignKey("Country")]
        public int Country_Id { get; set; }
        public Country Country { get; set; }
    }
}
