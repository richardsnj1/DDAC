﻿using DDAC.Data;
using DDAC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DDAC.Controllers
{
    public class HealthController : Controller
    {
        private readonly DDACContext _context;
        public HealthController(DDACContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            List<HealthRecords> recordlist = await _context.HealthRecords.ToListAsync();
            return View(recordlist);
        }
		[Authorize(Roles = "Parent")]
		public IActionResult AddRecord()
        {
            return View();
        }
		[Authorize(Roles = "Admin")]
		public IActionResult EditRecord()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Parent")]
		public async Task<IActionResult> AddData(HealthRecords health)
        {
            if (ModelState.IsValid)
            {
                _context.HealthRecords.Add(health);
                await _context.SaveChangesAsync();
                TempData["record"] = "Sick Reported";
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteRecord(int? rid)
        {
            if (rid == null)
            {
                return NotFound();
            }

            HealthRecords records = await _context.HealthRecords.FindAsync(rid);
            if (records == null)
            {
                return NotFound();

            }
            _context.HealthRecords.Remove(records);
            await _context.SaveChangesAsync();
            TempData["record"] = "Record Deleted";
            return RedirectToAction("Index", "Health");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditRecord(int? rid)
        {
            if (rid == null)
            {
                return NotFound();
            }

            HealthRecords records = await _context.HealthRecords.FindAsync(rid);
            if (records == null)
            {
                return NotFound();

            }
            records.sickDate = records.sickDate.Trim();

            return View(records);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateRecords(HealthRecords records)
        {
            if (ModelState.IsValid)
            {
                _context.HealthRecords.Update(records);
                await _context.SaveChangesAsync();
                TempData["record"] = "Record Updated";
                return RedirectToAction("Index", "Health");
            }
            return RedirectToAction("Index", "Health");
        }
    }
}
