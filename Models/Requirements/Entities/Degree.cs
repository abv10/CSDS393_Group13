using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Requirements.Entities
{
    public class Degree
    {
        [Key]
        public string DegreeId { get; set; }
        public string Name { get; set; }
        public string Concentration { get; set; }
        public int MaxOverlap { get; set; }
        public bool IsMajor { get; set; }
        public IList<CourseGroup> CourseGroups { get; set; }
        public IList<Course> Courses { get; set; }
    }
}
