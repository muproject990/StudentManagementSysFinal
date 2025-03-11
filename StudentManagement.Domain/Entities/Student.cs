using System;
using System.Collections;
using System.Collections.Generic;

namespace StudentManagement.Domain.Entities
{
    public class Student
    {
        public int StudentID { get; set; } // Primary Key
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }  // Must be unique
        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;



        // Navigation properties
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
