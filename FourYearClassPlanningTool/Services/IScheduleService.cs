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

    }
}
