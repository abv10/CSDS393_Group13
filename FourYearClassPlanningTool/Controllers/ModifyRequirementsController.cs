using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using Microsoft.EntityFrameworkCore;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using FourYearClassPlanningTool.Services;




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
                return View(_requirementsContext); 
            }
            else
            {
                return View("NotAdmin.cshtml");
            }

            
        }

        public IActionResult Edit(string DegreeID)
        {
            ViewData["courses"] = _requirementsContext.Find<Degree>(DegreeID).Courses;
            return RedirectToAction("Modify", "Modify");
        }
    }
}
