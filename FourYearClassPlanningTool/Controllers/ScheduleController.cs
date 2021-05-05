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
        private string userId;
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
            userId = User.Identity.Name;
            User user = ControllerHelpers.GetOrCreateUser(userId, _context);
            List<Degree> remainingCourses = _service.GetRemainingRequirements(userId, out string message);
            try
            {
                ViewData["Remaining"] = _service.AdjustRemainingRequirements(remainingCourses, user.Schedules.ToList());
            }
            catch(NullReferenceException e)
            {
                ViewData["Remaining"] = remainingCourses;
            }
            return View();
        }


        // GET: Schedule/CreateFutureSchedule/
        public IActionResult CreateFutureSchedule()
        {
            userId = User.Identity.Name;
            List<Degree> remainingRequirements = _service.GetRemainingRequirements(userId, out string message);
            List<Models.Requirements.Entities.Course> remainingCourses = new List<Models.Requirements.Entities.Course>();
            foreach (Degree d in remainingRequirements)
            {
                foreach(Models.Requirements.Entities.Course c1 in d.Courses)
                {
                    remainingCourses.Add(c1);
                }

                foreach(CourseGroup g in d.CourseGroups)
                {
                    foreach(Models.Requirements.Entities.Course c2 in g.Courses)
                    {
                        remainingCourses.Add(c2);
                    }
                }
            }
            ViewData["Courses"] = remainingCourses;
            return View();
        }

        // GET: Schedules
        public IActionResult Schedules()
        {
            userId = User.Identity.Name;
            User user = ControllerHelpers.GetOrCreateUser(userId, _context);
            ViewData["Schedules"] = user.Schedules;
            return View();
        }
        // GET: CompletedCourses
        public IActionResult CompletedCourses()
        {
            userId = User.Identity.Name;
            User user = ControllerHelpers.GetOrCreateUser(userId, _context);
            ViewData["Courses"] = user.CompletedCourses;
            return View();
        }
    }
}
