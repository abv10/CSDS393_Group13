using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Requirements.Entities
{
    public class Course
    {
        [Key]
        public string CourseId { get; set; }
        public string Name { get; set; }
        public string SemestersOffered { get; set; }
        public int Credits { get; set; }
        public IList<CourseGroup> CourseGroups { get; set; }
        public IList<Degree> Degrees { get; set; }

    }
}
