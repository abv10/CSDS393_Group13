using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Users.Entities
{
    public class Schedule
    {
        [Key]
        public string ScheduleID { get; set; }
        public string Semester { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
