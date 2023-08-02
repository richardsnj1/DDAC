using DDAC.Models;
using DDAC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDAC.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly DDACContext _context;
        public ActivitiesController(DDACContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EditActivites()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Index(Activities activities)
        {

            if (ModelState.IsValid)
            {
                _context.Activities.Add(activities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(activities);
        }
        }
    }
