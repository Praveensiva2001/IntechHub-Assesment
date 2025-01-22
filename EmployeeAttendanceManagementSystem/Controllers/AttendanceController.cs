

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
