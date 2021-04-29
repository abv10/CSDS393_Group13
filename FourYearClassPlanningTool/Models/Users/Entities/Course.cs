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
        public string CourseID { get; set; }
        public string Name { get; set; }
        public string SemesterOffered { get; set; }
        public int Credits { get; set; }
    }
}