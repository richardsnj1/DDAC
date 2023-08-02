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
        public async Task<IActionResult> Index()
        {
            List<Activities> activitieslist = await _context.Activities.ToListAsync();
            return View(activitieslist);
        }

        public IActionResult AddActivities()
        {
            return View();
        }
        public IActionResult EditActivites()
        {
            return View();
        }

        //AddActivities

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivities(Activities activities)
        {
            if (ModelState.IsValid)
            {
                _context.Activities.Add(activities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("AddActivities", activities);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivities(int? fid)
        {
            if (fid == null)
            {
                return NotFound();
            }

            Activities activities = await _context.Activities.FindAsync(fid);
            if (activities == null)
            {
                return NotFound();

            }
            _context.Activities.Remove(activities);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActivities(int? fid)
        {
            if (fid == null)
            {
                return NotFound();
            }

            Activities activities = await _context.Activities.FindAsync(fid);
            if (activities == null)
            {
                return NotFound();

            }

            return View(activities);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateActivities(Activities activities)
        {
            if (ModelState.IsValid)
            {
                _context.Activities.Update(activities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("AddActivities", activities);
        }

    }
}
