namespace StudentManagement.Domain.Entities
{
    public class Grade
    {
        public int GradeID { get; set; }    // Primary Key
        public int StudentID { get; set; }  // Foreign Key
        public int CourseID { get; set; }   // Foreign Key
        public char GradeLetter { get; set; }  // Allowed values: A, B, C, D, F

        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
