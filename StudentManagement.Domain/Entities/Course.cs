using System.Collections;
using System.Collections.Generic;

namespace StudentManagement.Domain.Entities
{
    public class Course
    {
        public int CourseID { get; set; } // Primary Key
        public string CourseName { get; set; }
        public string CourseCode { get; set; } // Unique field
        public int CreditHours { get; set; }   // Must be between 1 and 5

        // Navigation properties
        // Navigation properties
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
