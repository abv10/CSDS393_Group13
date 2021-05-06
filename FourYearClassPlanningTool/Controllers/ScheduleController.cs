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
            if (user == null)
            {
                ViewData["Message"] = "You must login to use this feature";
                return View("NullUser");
            }
            List<Degree> remainingCourses = _service.GetRemainingRequirements(userId, out string message);
            try
            {
                ViewData["Remaining"] = _service.AdjustRemainingRequirements(remainingCourses, user.Schedules.ToList());
            }
            catch(NullReferenceException e)
            {
                if (remainingCourses != null)
                    ViewData["Remaining"] = remainingCourses;
            }
            return View();
        }

        // GET: Schedules
        public IActionResult Schedules()
        {
            userId = User.Identity.Name;
            User user = ControllerHelpers.GetOrCreateUser(userId, _context);
            if (user == null)
            {
                ViewData["Message"] = "You must login to use this feature";
                return View("NullUser");
            }
            List<Degree> remainingRequirements = _service.GetRemainingRequirements(userId, out string message);
            var remainingCourses = RemainingCourses(remainingRequirements);

            ViewData["ErrorMessage"] = "";
            ViewData["Schedules"] = user.Schedules;
            ViewData["Courses"] = remainingCourses;
            return View();
        }

        private List<string> RemainingCourses(List<Degree> remainingRequirements)
        {
            List<string> remainingCourses = new List<string>();
            if(remainingRequirements == null || remainingRequirements.Count == 0)
            {
                return remainingCourses;
            }
            foreach (Degree d in remainingRequirements)
            {
                foreach (Models.Requirements.Entities.Course c1 in d.Courses)
                {
                    remainingCourses.Add(c1.Name);
                }

                foreach (CourseGroup g in d.CourseGroups)
                {
                    foreach (Models.Requirements.Entities.Course c2 in g.Courses)
                    {
                        remainingCourses.Add(c2.Name);
                    }
                }
            }
            return remainingCourses;
        }
        public IActionResult Remove(string courseId, string scheduleId)
        {
            var user = ControllerHelpers.GetOrCreateUser(User.Identity.Name, _context);
            if(user != null)
            {
                var schedule = user.Schedules.Where(s => s.ScheduleId.Equals(scheduleId)).First();
                var course = _context.Courses.Find(courseId);
                schedule.Courses.Remove(course);
                if (!_service.ValidateSchedule(user.UserId, user.Schedules.ToList(), out string errorMessage))
                {
                    ViewData["ErrorMessage"] = "Cannot remove " + course.Name + " because " + errorMessage;
                    schedule.Courses.Add(course);   
                }
                _context.Update(user);
                _context.SaveChanges();
                List<Degree> remainingRequirements = _service.GetRemainingRequirements(user.UserId, out string message);
                var remainingCourses = RemainingCourses(remainingRequirements);

                ViewData["Schedules"] = _context.Users.Find(User.Identity.Name).Schedules;
                ViewData["Courses"] = remainingCourses;
            }
            return View("Schedules");
        }

        public IActionResult Add(string courseName, string semesterId)
        {
            var user = ControllerHelpers.GetOrCreateUser(User.Identity.Name, _context);
            if (user != null)
            {
                var schedule = user.Schedules.Where(s => s.ScheduleId.Equals(semesterId)).First();

                var course = _context.Courses.Where(c => c.Name.Equals(courseName)).FirstOrDefault();
                if(course != null)
                {
                    schedule.Courses.Add(course);
                }

                if (!_service.ValidateSchedule(user.UserId, user.Schedules.ToList(), out string Errormessage))
                {
                    ViewData["ErrorMessage"] = "Cannot add " + course.Name + " because " + Errormessage;
                    schedule.Courses.Remove(course);
                }
                _context.Update(user);
                _context.SaveChanges();
                List<Degree> remainingRequirements = _service.GetRemainingRequirements(user.UserId, out string message);
                var remainingCourses = RemainingCourses(remainingRequirements);

                ViewData["Schedules"] = _context.Users.Find(User.Identity.Name).Schedules;
                ViewData["Courses"] = remainingCourses;
            }
            return View("Schedules");
        }
        // GET: CompletedCourses
        public IActionResult CompletedCourses()
        {
            userId = User.Identity.Name;
            User user = ControllerHelpers.GetOrCreateUser(userId, _context);
            if (user == null)
            {
                ViewData["Message"] = "You must login to use this feature";
                return View("NullUser");
            }
            ViewData["Courses"] = user.CompletedCourses;
            return View();
        }

        
    }
}
