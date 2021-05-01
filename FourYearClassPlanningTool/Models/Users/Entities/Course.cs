using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Users.Entities
{
    public class Course
    {
        [Key]
        public string CourseId { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public virtual ICollection<User> Users {get; set;}
        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}