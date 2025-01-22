using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendanceManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required] 
        public string Name { get; set; }

        [Required] 
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required] 
        public string Department { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}

