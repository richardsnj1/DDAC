using DDAC.Models;
using DDAC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DDAC.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly DDACContext _context;
        public ActivitiesController(DDACContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchActivity) 
        { 
 
        
            List<Activities> activitieslist = await _context.Activities.ToListAsync();

            //filtering
            if (!string.IsNullOrEmpty(searchActivity))
            {
                activitieslist = activitieslist.Where(a => a.activityName.Contains(searchActivity)).ToList();
			}

            return View(activitieslist);
        }

		[Authorize(Roles = "Teacher, Admin")]
		public IActionResult AddActivities()
        {
            return View();
        }

        //AddActivities

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Teacher, Admin")]
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
		[Authorize(Roles = "Teacher, Admin")]
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
		[Authorize(Roles = "Teacher, Admin")]
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
		[Authorize(Roles = "Teacher, Admin")]
		public async Task<IActionResult> UpdateActivities(Activities activities)
        {
            if (ModelState.IsValid)
            {
                _context.Activities.Update(activities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("EditActivities", activities);
        }

    }
}
