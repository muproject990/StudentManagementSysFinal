using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.App.DTOs
{
    public class CreateGradeDto
    {
        [Required(ErrorMessage = "Student Id is required")]
        public int StudentID { get; set; }  // Foreign Key

        [Required(ErrorMessage = "Course Id is required")]
        public int CourseID { get; set; }   // Foreign Key
        [Required(ErrorMessage = "GradeLetter  is required")]
        public char GradeLetter { get; set; }

    }
}