using FourYearClassPlanningTool.Models;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Models.Users;
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
        private readonly UsersContext _usersContext;

        public HomeController(ILogger<HomeController> logger, IScheduleService service, UsersContext usersContext)
        {
            _logger = logger;
            _service = service;
            _usersContext = usersContext;
        }

        public IActionResult Index()
        {
       /*     var user = ControllerHelpers.GetOrCreateUser(User.Identity.Name, _usersContext);
            if(user == null)
            {
                return View();
            }


            var x = _service.GetRemainingRequirements(user.UserId, out string errorMessage);
            if(x == null)
            {
                return View();
            }
            foreach(Degree d in x)
            {
                Debug.WriteLine(d.Name);
                Debug.WriteLine("Courses remaining: ");
                foreach(Course c in d.Courses)
                {
                    Debug.Write(c.Name + " ,");
                }
                Debug.WriteLine("CourseGroups remaining: ");
                foreach(CourseGroup g in d.CourseGroups)
                {
                    Debug.WriteLine(g.Name + ": Credits Remaining " + g.CreditsRequired + " : Courses Remaining " + g.CoursesRequired);
                    Debug.WriteLine("-");
                    foreach (Course c in g.Courses)
                    {
                        Debug.Write(c.Name + " ,");
                    }
                }
            }*/
            return View();
        }



       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
