using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Controllers
{
    public class CourseImportingController : Controller
    {
        private readonly UsersContext _contexts;

        public CourseImportingController(UsersContext context)
        {
            _contexts = context;
        }

        // GET: Courses
        public IActionResult Index()
        {
            return View(_contexts.Courses.ToList());
        }


        public IActionResult Add(string CourseId)
        {
            var course = _contexts.Courses.Find(CourseId);
            var user = _contexts.Users.Find(User.Identity.Name);
            user.CompletedCourses.Add(course);
            _contexts.Update(user);
            _contexts.SaveChanges();

            Index();

        }
    }
}
