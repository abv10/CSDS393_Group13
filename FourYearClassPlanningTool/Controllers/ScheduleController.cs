using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Services;

namespace FourYearClassPlanningTool.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly UsersContext _context;
        private readonly IScheduleService _service;
        public ScheduleController(UsersContext context, IScheduleService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {
            return View(await _context.Schedules.ToListAsync());
        }

        // GET: Schedule/FillRemainingRequirements/
        public IActionResult FillRemainingRequirements()
        {
            string id = User.Identity.Name;
            ViewData["Remaining"] = _service.GetRemainingRequirements(id, out string errorMessage);
            return View();
        }


        // GET: Schedule/CreateFutureSchedule/
        public IActionResult CreateFutureSchedule()
        {
            return View();
        }
    }
}
