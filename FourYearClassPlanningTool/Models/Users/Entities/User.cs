using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Users.Entities
{
    public class User
    {
        [Key]
        public string UserID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }
        public int Year { get; set; }
        public virtual ICollection<Course> CompletedCourses { get; set; }
    }
}