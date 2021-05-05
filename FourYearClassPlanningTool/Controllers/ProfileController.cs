using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using FourYearClassPlanningTool.Services;
using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;

namespace FourYearClassPlanningTool.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UsersContext _context;
        private readonly IScheduleService _service;
        private readonly RequirementsContext _reqContext;

        public ProfileController(UsersContext context, IScheduleService service, RequirementsContext reqContext)
        {
            _context = context;
            _service = service;
            _reqContext = reqContext;
        }

       
        private string GetDegrees(string unseparated)
        {
            if (string.IsNullOrEmpty(unseparated)){
                return unseparated;
            }
            string list = "";
            var split = unseparated.Split(';');
            foreach(var s in split)
            {
                var deg = _reqContext.Degrees.Find(s);
                if(deg.Concentration != "N/A")
                {
                    list += (deg.Name + ": " + deg.Concentration+ ",");
                }
                else
                {
                    list += (deg.Name + ",");
                }
            }
            return list.Substring(0, list.Length-1);
        }
        // GET: Profile/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //GET: Profile
        public async Task<IActionResult> Index()
        {


            var user = ControllerHelpers.GetOrCreateUser(User.Identity.Name, _context);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["PossibleDegrees"] = _reqContext.Degrees.ToList();
            ViewData["MyDegrees"] = GetMyDegrees(user.Major);
            return View(user);
        }

        public IActionResult AddDegree(string degreeId)
        {
            var user = ControllerHelpers.GetOrCreateUser(User.Identity.Name, _context);
            if (user != null)
            {
                _service.AddDegreeToUser(user.UserId, degreeId);
                ViewData["Major"] = GetDegrees(user.Major);
                ViewData["MyDegrees"] = GetMyDegrees(user.Major);
                ViewData["PossibleDegrees"] = _reqContext.Degrees.ToList();
            }
            return View("Index", user);
        }

        public IActionResult RemoveDegree(string degreeId)
        {
            var user = ControllerHelpers.GetOrCreateUser(User.Identity.Name, _context);
            if (user != null)
            {
                _service.RemoveDegreeFromUser(user.UserId, degreeId);
                ViewData["Major"] = GetDegrees(user.Major);
                ViewData["MyDegrees"] = GetMyDegrees(user.Major);
                ViewData["PossibleDegrees"] = _reqContext.Degrees.ToList();
            }
            return View("Index", user);
        }

        private List<Degree> GetMyDegrees(string unseparated)
        {
            List<Degree> degs = new List<Degree>();
            var split = unseparated.Split(';');
            foreach (var s in split)
            {
                degs.Add(_reqContext.Degrees.Find(s));
            }
            return degs;
        }

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,Name,Major,Year")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Profile/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
