using DDAC.Data;
using DDAC.Models;
using Microsoft.AspNetCore.Mvc;
using DDAC.Data;
using DDAC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDAC.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly DDACContext _context;
        public AttendanceController(DDACContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAttendance(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Attendance.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction("AllAttendance", "Enroll");
            }
            return View("AddAttendance", attendance);
        }
    }
}
//public class EnrollController : Controller
//{
//    private readonly DDACContext _context;

//    public EnrollController(DDACContext context)
//    {
//        _context = context;
//    }

//    public IActionResult Index()
//    {
//        return View();
//    }

//}