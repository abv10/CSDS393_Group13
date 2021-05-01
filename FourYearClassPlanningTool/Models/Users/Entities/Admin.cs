﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Users.Entities
{
    public class Admin
    {
        [Key]
        public string AdminID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        //Admins do not need to access Users at all
    }
}