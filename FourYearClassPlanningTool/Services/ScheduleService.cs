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

            /*var student = _userContext.Users.Where(u => u.UserId.Equals(studentId)).FirstOrDefault();
            if(student == null)
            {
                return null;
            }*/

            //var completedCourseIds = student.Courses.Select(c => c.CourseId);
            var completedCourseIds = new List<string>() { "MATH121","MATH122","MATH223","ENGR398","ENGL398","MATH201","CSDS391","MATH380", "STAT312"}.AsQueryable();

            //string[] degreeId = student.Major.split[","];
            string[] degreeIds = new string[] { "CSAI","MAMI" };
            List<Degree> degreesToReturn = new List<Degree>();
            var checkedCourses = new List<Course>();

            foreach (string degreeId in degreeIds)
            {
                Degree unmodified = _reqContext.Degrees.Find(new Object[] { degreeId });
                var uncompletedCourses = unmodified.Courses;
                for (int i = 0; i < uncompletedCourses.Count();)
                {
                    Course c = uncompletedCourses.ElementAt(i);
                    if (unmodified.MaxOverlap != 0 && completedCourseIds.Where(a => a.Equals(c.CourseId)).FirstOrDefault() != null)
                    {
                            uncompletedCourses.Remove(c);
                            checkedCourses.Add(c);
                            if(unmodified.MaxOverlap != -1)
                            {
                                if(checkedCourses.AsQueryable().Where(e => e.CourseId.Equals(c.CourseId)).FirstOrDefault() != null)
                                {
                                    unmodified.MaxOverlap--;
                                }
                            }
                      }
                        
                    else
                    {
                        i++;
                    }
                }
                var uncompletedCourseGroups = unmodified.CourseGroups;
                for (int i = 0; i < uncompletedCourseGroups.Count(); i++)
                {
                    CourseGroup group = uncompletedCourseGroups.ElementAt(i);
                    for (int j = 0; j < group.Courses.Count();)
                    {
                        Course c = group.Courses.ElementAt(j);
                        if (unmodified.MaxOverlap != 0 && completedCourseIds.Where(a => a.Equals(c.CourseId)).FirstOrDefault() != null)
                        {
                            group.Courses.Remove(c);
                            checkedCourses.Add(c);
                            if (group.CoursesRequired > 0)
                            {
                                group.CoursesRequired--;
                            }
                            if (group.CreditsRequired > 0)
                            {
                                group.CreditsRequired -= c.Credits;
                            }
                            if (unmodified.MaxOverlap != -1)
                            {
                                if (checkedCourses.AsQueryable().Where(e => e.CourseId.Equals(c.CourseId)).FirstOrDefault() != null)
                                {
                                    unmodified.MaxOverlap--;
                                }
                            }
                        }
                        else
                        {
                            j++;
                        }
                        if (group.CoursesRequired <= 0 & group.CreditsRequired <= 0)
                        {
                            unmodified.CourseGroups.Remove(group);
                            break;
                        }
                    }

                }
                degreesToReturn.Add(unmodified);
            }
                
             return degreesToReturn;
        }   
    }
}
