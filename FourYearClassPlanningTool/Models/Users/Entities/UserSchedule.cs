using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Users.Entities
{
    public class UserSchedule
    {
        [Key]
        public string UserID { get; set; }
        public string ScheduleID { get; set; }
    }
}
