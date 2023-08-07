using DDAC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDAC.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DDAC.Controllers
{
    public class EnrollController : Controller
    {
        private readonly DDACContext _context;
        public EnrollController(DDACContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllStudent(string searchStudent)
        {
            //var attdate = await _context.Enroll.Select(e => e.attDate).ToListAsync();
            //if (attdate.IsNullOrEmpty) { 

            //}
            List<Enroll> studentlist = await _context.Enroll.ToListAsync();

            //filtering
            if (!string.IsNullOrEmpty(searchStudent))
            {
                studentlist = studentlist.Where(a => a.childName.Contains(searchStudent)).ToList();
            }

            return View(studentlist);
		}
		[HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Parent")]
		public async Task<IActionResult> Enrollment(Enroll enroll)
        {
            if (ModelState.IsValid)
            {
                _context.Enroll.Add(enroll);
                await _context.SaveChangesAsync();
                TempData["enrolled"] = "Child Enrolled";
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }
    }
}