using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Requirements.Entities
{
    public class CourseGroup
    {
        [Key]
        public string CourseGroupId { get; set; }
        public string Name { get; set; }
        public int CoursesRequired { get; set; }
        public int CreditsRequired { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Degree> Degrees { get; set; }

    }
}
