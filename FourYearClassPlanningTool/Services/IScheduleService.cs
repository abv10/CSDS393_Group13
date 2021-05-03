using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Models.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Services
{
    public interface IScheduleService
    {
        public List<Degree> GetRemainingRequirements(string studentId, out string message);
        public bool ValidateSchedule(string studentId, List<Schedule> unaddedSchedules, out string message);
        public void AddScheduleToUser(string userId, Schedule toAdd);
        public void AddCoursesFromSchedule(string userId, string scheduleId);
        public void AddCourseGroupToDegree(string DegreeId, CourseGroup group);
        public void AddCourseToDegree(string DegreeId, Models.Requirements.Entities.Course course);
        public void AddCourseToCourseGroup(string CourseGroupId, Models.Requirements.Entities.Course course);
    }
}
