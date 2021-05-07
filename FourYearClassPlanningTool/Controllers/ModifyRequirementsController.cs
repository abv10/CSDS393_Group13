using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using Microsoft.EntityFrameworkCore;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using FourYearClassPlanningTool.Services;
using System.Linq;
using System.Collections.Generic;




namespace FourYearClassPlanningTool.Controllers
{
    public class ModifyRequirementsController : Controller
    {

        private readonly RequirementsContext _requirementsContext;
        private readonly UsersContext _usersContext;
        public ModifyRequirementsController(RequirementsContext requirements, UsersContext users)
        {
            _requirementsContext = requirements;
            _usersContext = users;
        }

        public IActionResult Index()
        {
            if (ControllerHelpers.IsAdmin(User.Identity.Name, _usersContext))
            {
                IList<Degree> degrees = _requirementsContext
                    .Degrees
                    .Select(d => d)
                    .ToList();
                return View(degrees); 
            }
            else
            {
                return View("NotAdmin");
            }

            
        }

        public IActionResult Edit(string DegreeID)
        {
            var degree = _requirementsContext.Find<Degree>(DegreeID);
            return View("Modify", degree);
        }

        public IActionResult Remove(string DegreeID, string CourseID)
        {
            var degree = _requirementsContext.Degrees.Find(DegreeID);
            var course = _requirementsContext.Courses.Find(CourseID);

            degree.Courses.Remove(course);
            foreach(CourseGroup g in degree.CourseGroups)
            {
                g.Courses.Remove(course);
            }
            _requirementsContext.Update(degree);
            _requirementsContext.SaveChanges();
            return View("Modify", degree);
        }
        public IActionResult Close()
        {

            return View("Index");
        }
    }
}
