using EmployeeAttendanceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Department> Departments { get; set; }
    }
}
