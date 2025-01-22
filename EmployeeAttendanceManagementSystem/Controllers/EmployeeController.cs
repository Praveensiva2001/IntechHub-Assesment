using EmployeeAttendanceManagementSystem.Data;
using EmployeeAttendanceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This method shows all Employees from the database who aren't deleted
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Where(e => e.IsDeleted == false).ToListAsync();
            return View(employees);
        }

        // This method shows the employee/create page for letting the user create a employee
        public IActionResult Create()
        {
            // This is used to Fetch departments from the database which i have inserted values there itself
            var departments = _context.Departments
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Name
                })
                .ToList();

            // In create views, department values fro databse is passed and shown in a dropdown
            ViewData["Departments"] = departments;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // This method gets the value from the form and stores in database
        public async Task<IActionResult> Create([Bind("Name,Email,Department")] Employee employee)
        {
            if (_context.Employees.Any(e => e.Email == employee.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate departments if ModelState is invalid
            ViewData["Departments"] = _context.Departments
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Name
                })
                .ToList();

            return View(employee);
        }

        // This method shows the Edit page and before showing it checks some conditions
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || employee.IsDeleted)
            {
                return NotFound();
            }

            // Fetch departments from the database
            ViewBag.Departments = _context.Departments
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Name
                })
                .ToList();

            return View(employee);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        // It receives input from the Form and checks condition and updates in database
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,Email,Department")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // To Handle general exceptions if needed
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate departments if ModelState is invalid
            ViewBag.Departments = _context.Departments
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Name
                })
                .ToList();

            return View(employee);
        }






        // This deletes an employee after checks whether employee is stored in database or not
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || employee.IsDeleted)
            {
                return NotFound();
            }

            employee.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
    }
}
