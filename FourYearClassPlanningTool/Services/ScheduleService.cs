using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Services
{
    public class ScheduleService : IScheduleService
    {
        RequirementsContext _reqContext;
        UsersContext _usersContext;
        public ScheduleService(RequirementsContext requirementsContext, UsersContext usersContext)
        {
            _reqContext = requirementsContext;
            _usersContext = usersContext;
        }

        public List<Degree> GetRemainingRequirements(string studentId, out string errorMessage)
        {
            var student = _usersContext.Users.Where(u => u.UserId.Equals(studentId)).FirstOrDefault();
            if(student == null)
            {
                errorMessage = "Unable to find a student with that id";
                return null;
            }
            var completedCourseIds = student.CompletedCourses.Select(c => c.CourseId);
            string[] degreeIds = student.Major.Split(";");
            List<Degree> degreesToReturn = new List<Degree>();
            var checkedCourses = new List<Models.Requirements.Entities.Course>();

            foreach (string degreeId in degreeIds)
            {
                Degree unmodified = _reqContext.Degrees.Find(new Object[] { degreeId });
                if(unmodified == null)
                {
                    errorMessage = "Invalid Degree Id: " + degreeId;
                    return null;
                }
                var uncompletedCourses = unmodified.Courses;
                for (int i = 0; i < uncompletedCourses.Count();)
                {
                    Models.Requirements.Entities.Course c = uncompletedCourses.ElementAt(i);
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
                        Models.Requirements.Entities.Course c = group.Courses.ElementAt(j);
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
                            i--;
                            break;
                        }
                    }

                }
                degreesToReturn.Add(unmodified);
            }
            errorMessage = null;
             return degreesToReturn;
        }   
    
        public bool ValidateSchedule(string studentId, List<Schedule> unaddedSchedules)
        {
            
            var student = _usersContext.Users.Find(new object[] { studentId });
            var completedCourses = student.CompletedCourses;

            var includeSchedulesAlreadyInDatabase = unaddedSchedules.ToList();
            includeSchedulesAlreadyInDatabase.AddRange(student.Schedules);
            var sortedSchedules = SortSchedules(includeSchedulesAlreadyInDatabase);

            for(int i = 0; i < sortedSchedules.Length; i++)
            {
                Schedule s = sortedSchedules[i];
                foreach (var c in sortedSchedules[i].Courses)
                {
                    if (!ValidSemester(c.CourseId, s.Semester))
                    {
                        return false;
                    }
                    string prerequisites = _reqContext.Courses.Find(new object[] { c.CourseId }).Prequisites.Replace(" ", "");
                    var andSplit = prerequisites.Split("and");
                    foreach (var p in andSplit)
                    {
                        bool prereqTaken = false;
                        var orSplit = p.Split("or");
                        foreach(var o in orSplit)
                        {
                            if(completedCourses.Where(com=> com.CourseId.Equals(o)).Any())
                            {
                                prereqTaken = true;
                                break;
                            }
                            for(int j = 0; j < i; j++)
                            {
                                if(sortedSchedules[j].Courses.Where(c => c.CourseId.Equals(o)).Any())
                                {
                                    prereqTaken = true;
                                    break;
                                }
                            }
                            if (prereqTaken)
                            {
                                break;
                            }
                        }
                        if (!prereqTaken)
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private bool ValidSemester(string courseID, string semester)
        {
            var semestersOffered = _reqContext.Courses.Find(new object[] { courseID }).SemestersOffered;
            if (semestersOffered.ToLower().Equals("both"))
            {
                return true;
            }
            else if (semestersOffered.ToLower().Equals("spring") && semester.ToLower().StartsWith("spring"))
            {
                return true;
            }
            else if (semestersOffered.ToLower().Equals("fall") && semester.ToLower().StartsWith("fall"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sorts schedules according to their semester
        /// </summary>
        /// <param name="includeSchedulesAlreadyInDatabase"></param>
        /// <returns></returns>
        private Schedule[] SortSchedules(List<Schedule> includeSchedulesAlreadyInDatabase)
        {
            var scheduleArray = includeSchedulesAlreadyInDatabase.ToArray();
            int n = scheduleArray.Length;
            for(int i = 0; i < n-1; i++)
            {
                int min_indx = i;
                for(int j = i+1; j <n; j++)
                {
                    if (SemesterToNumber(scheduleArray[j].Semester) < SemesterToNumber(scheduleArray[min_indx].Semester))
                    {
                        min_indx = j;
                    }
                }

                var temp = scheduleArray[min_indx];
                scheduleArray[min_indx] = scheduleArray[i];
                scheduleArray[i] = temp;
            }
            return scheduleArray;
        }

        /// <summary>
        /// Gets a number associated with a string of Semester ie Fall2019 or String2020
        /// </summary>
        /// <param name="semester"></param>
        /// <returns>a double associated with the semester</returns>
        private double SemesterToNumber(string semester)
        {
            string yearString = semester.Trim().Substring(semester.Length - 4);
            double year = (double)int.Parse(yearString);
            string seasonString = semester.Trim().Substring(0, semester.Length - 4);
            if (seasonString.ToLower().Equals("Fall"))
            {
                return year + 0.5; //Fall semester is higher than the spring of the same year
            }
            else
            {
                return year;
            }
        }
    }
}
