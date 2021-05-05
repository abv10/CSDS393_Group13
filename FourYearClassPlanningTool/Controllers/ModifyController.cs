using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Models.Requirements;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FourYearClassPlanningTool.Controllers
{
    public class ModifyController : Controller
    {
        public Degree degree;
        private RequirementsContext _requirementsContext;

        public ModifyController(RequirementsContext rq, string degreeId)
        {
            _requirementsContext = rq;
            degree = _requirementsContext.Find<Degree>(degreeId);
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Modify()
        {
            return View();
        }

        public IActionResult Remove(Course course)
        {
            degree.Courses.Remove(course);
            _requirementsContext.Update(degree);
            _requirementsContext.SaveChanges();
            return View();
        }

        public IActionResult Close()
        {

            return View("ModifyRequirements.cshtml");
        }
    }
}
