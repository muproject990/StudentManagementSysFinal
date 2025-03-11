using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.App.DTOs
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "CourseName is required")]

        public string CourseName { get; set; }
        [Required(ErrorMessage = "Course Code is required")]
        public string CourseCode { get; set; } // Unique field

        [Required(ErrorMessage = "CreditHours is required")]
        public int CreditHours { get; set; }   // Must be between 1 and 5

    }
}