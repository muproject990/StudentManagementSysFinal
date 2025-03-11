using System;

namespace StudentManagement.Domain.Entities
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }  // Primary Key
        public int StudentID { get; set; }       // Foreign Key
        public int CourseID { get; set; }        // Foreign Key
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Navigation properties// Navigation properties

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
