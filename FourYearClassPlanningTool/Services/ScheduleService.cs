using FourYearClassPlanningTool.Data;
using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly RequirementsContext _reqContext;
        private readonly UsersContext _usersContext;
        public ScheduleService(RequirementsContext requirementsContext, UsersContext usersContext)
        {
            _reqContext = requirementsContext;
            _usersContext = usersContext;
        }

        /// <summary>
        /// ***Note****
        /// 
        /// ***Make sure to validate schedule before calling this method***
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="toAdd"></param>
        public void AddScheduleToUser(string userId, Schedule toAdd)
        {
            var user = _usersContext.Users.Find(new object[] { userId });
            user.Schedules.Add(toAdd);
            _usersContext.Update(user);
            _usersContext.SaveChanges();
        }

        /// <summary>
        /// ***Note****
        /// 
        /// ***Make sure to validate schedule before calling this method***
        /// 
        /// Adds all of the courses from a given schedule to completed courses and removes the schedule for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="toAdd"></param>
        public void AddCoursesFromSchedule(string userId, string scheduleId)
        {
            var user = _usersContext.Users.Find(new object[] { userId });

            var schedule = user?.Schedules.Where(s => s.ScheduleId.Equals(scheduleId)).FirstOrDefault();

            if (schedule == null)
            {
                throw new Exception("Cannot find given schedule for that user");
            }
            foreach (var c in schedule.Courses.AsEnumerable())
            {
                if (!user.CompletedCourses.Contains(c))
                {
                    user.CompletedCourses.Add(c);
                }
            }
            user.Schedules.Remove(schedule);
            _usersContext.Update(user);
            _usersContext.SaveChanges();
        }
        public List<Degree> GetRemainingRequirements(string studentId, out string errorMessage)
        {
            var student = _usersContext.Users.Where(u => u.UserId.Equals(studentId)).FirstOrDefault();
            if (student == null)
            {
                errorMessage = "Unable to find a student with that id";
                return null;
            }
            var completedCourseIds = student.CompletedCourses?.Select(c => c.CourseId);
            if (completedCourseIds == null)
            {
                completedCourseIds = new List<string>();
            }
            if (student.Major == null)
            {
                errorMessage = "You must have an major to use this function";
                return null;
            }
            string[] degreeIds = student.Major.Split(";");
            List<Degree> degreesToReturn = new List<Degree>();
            var checkedCourses = new List<Models.Requirements.Entities.Course>();

            foreach (string degreeId in degreeIds)
            {
                Degree unmodified = _reqContext.Degrees.Find(new Object[] { degreeId });
                if (unmodified == null)
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

        public List<Degree> AdjustRemainingRequirements(List<Degree> degrees, List<Schedule> schedules)
        {
            if (degrees == null || degrees.Count == 0 || schedules == null || schedules.Count == 0)
            {
                throw new NullReferenceException();
            }
            var scheduleCourseIds = new List<string>();
            foreach (var schedule in schedules)
            {
                scheduleCourseIds.AddRange(schedule.Courses.Select(s => s.CourseId).ToList());
            }
            var checkedCourses = new List<Models.Requirements.Entities.Course>();
            foreach (Degree degree in degrees)
            {
                var uncompletedCourses = degree.Courses;
                for (int i = 0; i < uncompletedCourses.Count();)
                {
                    Models.Requirements.Entities.Course c = uncompletedCourses.ElementAt(i);
                    if (degree.MaxOverlap != 0 && scheduleCourseIds.Where(a => a.Equals(c.CourseId)).FirstOrDefault() != null)
                    {
                        uncompletedCourses.Remove(c);
                        checkedCourses.Add(c);
                        if (degree.MaxOverlap != -1)
                        {
                            if (checkedCourses.AsQueryable().Where(e => e.CourseId.Equals(c.CourseId)).FirstOrDefault() != null)
                            {
                                degree.MaxOverlap--;
                            }
                        }
                    }

                    else
                    {
                        i++;
                    }
                }
                var uncompletedCourseGroups = degree.CourseGroups;
                for (int i = 0; i < uncompletedCourseGroups.Count(); i++)
                {
                    CourseGroup group = uncompletedCourseGroups.ElementAt(i);
                    for (int j = 0; j < group.Courses.Count();)
                    {
                        Models.Requirements.Entities.Course c = group.Courses.ElementAt(j);
                        if (degree.MaxOverlap != 0 && scheduleCourseIds.Where(a => a.Equals(c.CourseId)).FirstOrDefault() != null)
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
                            if (degree.MaxOverlap != -1)
                            {
                                if (checkedCourses.AsQueryable().Where(e => e.CourseId.Equals(c.CourseId)).FirstOrDefault() != null)
                                {
                                    degree.MaxOverlap--;
                                }
                            }
                        }
                        else
                        {
                            j++;
                        }
                        if (group.CoursesRequired <= 0 & group.CreditsRequired <= 0)
                        {
                            degree.CourseGroups.Remove(group);
                            i--;
                            break;
                        }
                    }

                }
            }
            return degrees;
        }
        public bool ValidateSchedule(string studentId, List<Schedule> unaddedSchedules, out string message)
        {

            var student = _usersContext.Users.Find(new object[] { studentId });
            var completedCourses = student.CompletedCourses;

            var includeSchedulesAlreadyInDatabase = unaddedSchedules.ToList();
            includeSchedulesAlreadyInDatabase.AddRange(student.Schedules);
            var sortedSchedules = SortSchedules(includeSchedulesAlreadyInDatabase);

            for (int i = 0; i < sortedSchedules.Length; i++)
            {
                Schedule s = sortedSchedules[i];
                foreach (var c in sortedSchedules[i].Courses)
                {
                    if (!ValidSemester(c.CourseId, s.Semester))
                    {
                        message = "You cannot take " + c.Name + "in semester: " + s.Semester;
                        return false;
                    }
                    string prerequisites = _reqContext.Courses.Find(new object[] { c.CourseId }).Prequisites.Replace(" ", "");
                    if (prerequisites.ToLower().Equals("none"))
                    {
                        continue;
                    }
                    var andSplit = prerequisites.Split("and");
                    foreach (var p in andSplit)
                    {
                        bool prereqTaken = false;
                        var orSplit = p.Split("or");
                        foreach (var o in orSplit)
                        {
                            if (completedCourses.Where(com => com.CourseId.Equals(o)).Any())
                            {
                                prereqTaken = true;
                                break;
                            }
                            for (int j = 0; j < i; j++)
                            {
                                if (sortedSchedules[j].Courses.Where(c => c.CourseId.Equals(o)).Any())
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
                            message = c.Name + " does not have prerequisite of " + p;
                            return false;
                        }
                    }
                }
            }
            message = "Valid Schedule";
            return true;
        }

        /// <summary>
        /// Determines if a course can be taken in the semester for which it is scheduled
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="semester"></param>
        /// <returns>True if the course can be taken in the semester, else false</returns>
        private bool ValidSemester(string courseID, string semester)
        {
            var semestersOffered = _reqContext.Courses.Find(new object[] { courseID })?.SemestersOffered;
            if (semestersOffered == null)
            {
                return false;
            }
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
            for (int i = 0; i < n - 1; i++)
            {
                int min_indx = i;
                for (int j = i + 1; j < n; j++)
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
            if (seasonString.ToLower().Equals("fall"))
            {
                return year + 0.5; //Fall semester is higher than the spring of the same year
            }
            else
            {
                return year;
            }
        }

        public void AddCourseToCourseGroup(string CourseGroupId, Models.Requirements.Entities.Course course)
        {
            if (course == null)
            {
                throw new NullReferenceException();
            }
            var courseGroup = _reqContext.CourseGroups.Find(CourseGroupId);
            if (courseGroup == null)
            {
                throw new NullReferenceException();
            }
            courseGroup.Courses.Add(course);
            _reqContext.SaveChanges();
        }

        public void AddCourseToDegree(string DegreeId, Models.Requirements.Entities.Course course)
        {
            if (course == null)
            {
                throw new NullReferenceException();
            }
            var degree = _reqContext.Degrees.Find(DegreeId);
            if (degree == null)
            {
                throw new NullReferenceException();
            }
            degree.Courses.Add(course);
            _reqContext.SaveChanges();
        }

        public void AddCourseGroupToDegree(string DegreeId, CourseGroup group)
        {
            if (group == null)
            {
                throw new NullReferenceException();
            }
            var degree = _reqContext.Degrees.Find(DegreeId);
            if (degree == null)
            {
                throw new NullReferenceException();
            }
            degree.CourseGroups.Add(group);
            _reqContext.SaveChanges();
        }

        public List<string> SearchDegrees(string searchString)
        {
            var degreeStrings = new List<string>();
            searchString = searchString.ToLower();
            var returnedDegrees = _reqContext.Degrees.Where(d => (d.DegreeId.ToLower().Contains(searchString)) || (d.Name.ToLower().Contains(searchString)) || (d.Concentration.ToLower().Contains(searchString)));
            if (returnedDegrees.Count() != 0)
            {
                foreach (var d in returnedDegrees.AsEnumerable())
                {
                    degreeStrings.Add(d.DegreeId + ", " + d.Name + ", " + d.Concentration);
                }
            }
            return degreeStrings;
        }

        public void AddDegreeToUser(string userId, string degreeId)
        {
            if (!_reqContext.Degrees.Any(d => d.DegreeId.Equals(degreeId)))
            {
                throw new Exception("Cannot add Degree");
            }
            var user = _usersContext.Users.Find(userId);
            if (user != null)
            {
                user.Major = user.Major + ";" + degreeId;
            }
            else
            {
                throw new Exception("Invalid user, cannot add Degree");
            }
            _usersContext.Update(user);
            _usersContext.SaveChanges();
        }

        public void RemoveDegreeFromUser(string userId, string degreeId)
        {
            var user = _usersContext.Users.Find(userId);
            if (user != null)
            {
                if (user.Major.Contains(degreeId))
                {
                    user.Major = user.Major.Replace(degreeId, "");
                    user.Major = user.Major.Replace(";;", ";");
                    if (user.Major.EndsWith(';'))
                    {
                        user.Major = user.Major.Substring(0, user.Major.Length - 1);
                    }
                }
                else
                {
                    throw new Exception("User does not have that degree");
                }
            }

            else
            {
                throw new Exception("Invalid user, cannot add Degree");
            }
            _usersContext.Update(user);
            _usersContext.SaveChanges();
        }
    }
}