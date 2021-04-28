using FourYearClassPlanningTool.Models.Requirements.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Requirements
{
    public class RequirementsContext: DbContext
    {
        public RequirementsContext (DbContextOptions<RequirementsContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<Course> Courses { get; set; } 
    }
}
