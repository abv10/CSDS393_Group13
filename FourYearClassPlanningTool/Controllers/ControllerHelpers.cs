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
                context.Users.Add(new User() { UserId = userName, CompletedCourses = new List<Course>(), Schedules = new List<Schedule>() });
                context.SaveChanges();
                return context.Users.Find(userName);
            }
        }
    }
}
