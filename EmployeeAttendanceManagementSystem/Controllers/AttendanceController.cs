//using EmployeeAttendanceManagementSystem.Data;
//using EmployeeAttendanceManagementSystem.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;

//namespace EmployeeAttendanceManagementSystem.Controllers
//{
//    public class AttendanceController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public AttendanceController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index(DateTime? date)
//        {
//            // Default to today's date if no date is selected
//            DateTime selectedDate = date ?? DateTime.Now.Date;

//            // Get all employees not marked as deleted
//            var employees = _context.Employees
//                                     .Where(e => !e.IsDeleted)
//                                     .ToList();

//            // Get attendance records for the selected date
//            var attendances = _context.Attendances
//                                       .Where(a => a.Date == selectedDate)
//                                       .Include(a => a.Employee)
//                                       .ToList();

//            // Combine employees and attendance for the view
//            var attendanceViewModel = employees.Select(e => new
//            {
//                Employee = e,
//                Attendance = attendances.FirstOrDefault(a => a.EmployeeId == e.EmployeeId),
//            });

//            ViewBag.SelectedDate = selectedDate;

//            return View(attendanceViewModel);
//        }

//        [HttpPost]
//        public IActionResult CheckIn(int employeeId)
//        {
//            var attendance = new Attendance
//            {
//                EmployeeId = employeeId,
//                Date = DateTime.Now.Date,
//                CheckInTime = DateTime.Now,
//            };

//            _context.Attendances.Add(attendance);
//            _context.SaveChanges();

//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        public IActionResult CheckOut(int attendanceId)
//        {
//            var attendance = _context.Attendances.FirstOrDefault(a => a.AttendanceId == attendanceId);
//            if (attendance != null && attendance.CheckOutTime == null)
//            {
//                attendance.CheckOutTime = DateTime.Now;
//                _context.SaveChanges();
//            }

//            return RedirectToAction("Index");
//        }

//        // POST: Attendance/Delete
//        //[HttpPost]
//        //public IActionResult Delete(int id)
//        //{
//        //    var attendance = _context.Attendances.FirstOrDefault(a => a.AttendanceId == id);

//        //    if (attendance != null)
//        //    {
//        //        attendance.IsDeleted = true;
//        //        _context.SaveChanges();
//        //    }

//        //    return RedirectToAction("Index");
//        //}
//    }
//}



using EmployeeAttendanceManagementSystem.Data;
using EmployeeAttendanceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace EmployeeAttendanceManagementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This is the default page of showing the registered employee and their checkin and checkout
        public async Task<IActionResult> Index(DateTime? selectedDate)
        {
            var today = DateTime.Today;
            var attendanceDate = selectedDate ?? today;

            var employeesWithAttendance = await _context.Employees
                .Where(e => !e.IsDeleted) 
                .Select(e => new
                {
                    e.EmployeeId,
                    e.Name,
                    e.Department,
                    Attendance = _context.Attendances.FirstOrDefault(a => a.EmployeeId == e.EmployeeId && a.Date == attendanceDate)
                })
                .ToListAsync();

            ViewData["SelectedDate"] = attendanceDate;
            return View(employeesWithAttendance);
        }


        [HttpPost]
        // 
        public async Task<IActionResult> CheckIn(int employeeId)
        {
            var today = DateTime.Today;
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == today);

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    Date = today,
                    CheckInTime = DateTime.Now
                };
                _context.Attendances.Add(attendance);
            }
            else
            {
                attendance.CheckInTime = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(int employeeId)
        {
            var today = DateTime.Today;
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == today);

            if (attendance != null)
            {
                attendance.CheckOutTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }

}