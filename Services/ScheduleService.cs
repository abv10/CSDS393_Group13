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
            var completedCourseIds = new List<string>() { "MATH121","CSDS391","MATH380", "STAT312"}.AsQueryable();
            //
            string degreeId = "CSAI";
            Degree unmodified = _reqContext.Degrees.Find(new Object[] { degreeId });
            List<Degree> degreesToReturn = new List<Degree>();
            var uncompletedCourses = unmodified.Courses;
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
            var uncompletedCourseGroups = unmodified.CourseGroups;
            for(int i = 0; i < uncompletedCourseGroups.Count(); i++)
            {
                CourseGroup group = uncompletedCourseGroups.ElementAt(i);
                for(int j = 0; j < group.Courses.Count(); )
                {
                    Course c = group.Courses.ElementAt(j);
                    if (completedCourseIds.Where(a => a.Equals(c.CourseId)).FirstOrDefault() != null)
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
                        if (group.CoursesRequired <= 0 & group.CreditsRequired <= 0)
                        {
                            unmodified.CourseGroups.Remove(group);
                            break;
                        }
                    }
                    else
                    {
                        j++;
                    }
                }

            }

            degreesToReturn.Add(unmodified);
            return degreesToReturn;
        }
    }
}
