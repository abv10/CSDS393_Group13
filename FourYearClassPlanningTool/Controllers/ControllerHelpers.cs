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
                context.Users.Add(new User() { UserId = userName, CompletedCourses = new List<Course>(), Schedules = new List<Schedule>()});
                context.SaveChanges();
                return context.Users.Find(userName);
            }
        }

        public static bool IsAdmin(string userName, UsersContext context)
        {
            return context.Admins.Any(a => a.AdminID == userName);
        }
    }
}
