using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;



namespace FourYearClassPlanningTool.Controllers
{
    public class ModifyRequirementsController : Controller
    {

        private readonly RequirementsContext _requirementsContext;
        public ModifyRequirementsController(RequirementsContext requirements)
        {
            _requirementsContext = requirements;
        }

        public IActionResult Index()
        {
            return View(_requirementsContext);
        }

        public IActionResult Edit(string DegreeID)
        {
            ViewData["courses"] = _requirementsContext.Find<Degree>(DegreeID).Courses;
            return RedirectToAction("Modify", "Modify");
        }
    }
}
