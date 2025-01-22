using System;
using System.Collections.Generic;
using EmployeeAttendanceManagementSystem.Models;


namespace EmployeeAttendanceManagementSystem.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
