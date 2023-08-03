using DDAC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDAC.Data;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
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