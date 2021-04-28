using FourYearClassPlanningTool.Models.Requirements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Services
{
    public interface IScheduleService
    {
        public ICollection<Degree> GetRemainingRequirements(string studentId);
    }
}
