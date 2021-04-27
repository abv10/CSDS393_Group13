using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Services
{
    public class ScheduleService : IScheduleService
    {
        RequirementsContext _reqContext; 
        public ScheduleService(RequirementsContext context)
        {
            _reqContext = context;
        }
        public ICollection<Degree> GetRemainingRequirements(string studentId)
        {
            //Get Student
            //Get Completed Courses
            //IQueryable<Course>
            var completedCourseIds = new List<string>().AsQueryable();
            //
            string degreeId = "CSAI";
            Degree unmodified = _reqContext.Degrees.Find(new Object[] { degreeId });
            //
            var uncompletedCourses = unmodified.Courses.ToList();
            var checkedCourses = new List<Course>();
            for (int i = 0; i < uncompletedCourses.Count(); )
            {
                Course c = uncompletedCourses.ElementAt(i);
                if (completedCourseIds.Where(a => a.Equals(c.CourseId)) != null)
                {
                    uncompletedCourses.Remove(c);
                    checkedCourses.Add(c);
                }
                else
                {
                    i++;
                }
            }
            var uncompletedCourseGroups = _reqContext.CourseGroups.ToList();
            for(int i = 0; i < uncompletedCourseGroups.Count(); i++)
            {
                CourseGroup group = uncompletedCourseGroups.ElementAt(0);
                for(int j = 0; j < group.Courses.Count(); i++)
                {
                    Course c = group.Courses.ElementAt(i);
                    if (completedCourseIds.Where(a => a.Equals(c.CourseId)) != null)
                    {
                        group.Courses.Remove(c);
                        checkedCourses.Add(c);
                        if(group.CoursesRequired > 0)
                        {
                            group.CoursesRequired--;
                        }
                        if(group.CreditsRequired > 0)
                        {
                            group.CreditsRequired -= c.Credits;
                        }
                    }
                    else
                    {
                        j++;
                    }
                }

            }

            return null;
        }
    }
}
