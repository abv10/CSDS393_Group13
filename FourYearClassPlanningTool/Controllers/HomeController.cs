using FourYearClassPlanningTool.Models;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScheduleService _service;

        public HomeController(ILogger<HomeController> logger, IScheduleService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            var x = _service.GetRemainingRequirements("Filler");
            foreach(Degree d in x)
            {
                Console.WriteLine(d.Name);
                Console.WriteLine("Courses remaining: ");
                foreach(Course c in d.Courses)
                {
                    Console.Write(c.Name + " ,");
                }
                Console.WriteLine("CourseGroups remaining: ");
                foreach(CourseGroup g in d.CourseGroups)
                {
                    Console.WriteLine(g.Name + ": Credits Remaining " + g.CreditsRequired + " : Courses Remaining " + g.CoursesRequired);
                    Console.WriteLine("-");
                    foreach (Course c in g.Courses)
                    {
                        Console.Write(c.Name + " ,");
                    }
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
