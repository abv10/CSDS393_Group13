using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Controllers
{
    public static class ControllerHelpers
    {
        /// <summary>
        /// Gets the User from the usersContext given a string (creates one if none exists)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static User GetOrCreateUser(string userName, UsersContext context)
        {
            if(String.IsNullOrWhiteSpace(userName))
            {
                return null;
            }
            var user = context.Users.Find(userName);
            if(user != null)
            {
                return user;
            }
            else
            {
                context.Users.Add(new User() { UserId = userName, CompletedCourses = new List<Course>(), Schedules = CreateEmptySchedules(userName) });
                context.SaveChanges();
                return context.Users.Find(userName);
            }
        }

        private static ICollection<Schedule> CreateEmptySchedules(string userName)
        {
            var schedules = new List<Schedule>();
            var username = userName.Split("@")[0];
            var month = DateTime.UtcNow.Month;
            var year = DateTime.UtcNow.Year;
            int i = 0;
            if(month > 8)
            {
                i = 1;
            }
            for(int j = 0; j <= 4; )
            {
                var yearString = (year + j).ToString();
                var season = "";
                if(i == 0)
                {
                    season = "Spring";
                    i = 1;
                }
                else
                {
                    season = "Fall";
                    i = 1;
                    j++;
                }
                schedules.Add(new Schedule()
                {
                    ScheduleId = season + yearString + username,
                    Courses = new List<Course>()
                });
            }
            return schedules;
        }

        public static bool IsAdmin(string userName, UsersContext context)
        {
            return context.Admins.Any(a => a.AdminID == userName);
        }
    }
}
