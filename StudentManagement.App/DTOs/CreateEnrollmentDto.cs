using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.App.DTOs
{
    public class CreateEnrollmentDto
    {

        [Required(ErrorMessage = "StudentId is required")]
        public int StudentID { get; set; }       // Foreign Key
        [Required(ErrorMessage = "CourseId is required")]
        public int CourseID { get; set; }        // Foreign Key

        [Required(ErrorMessage = "EnrollmentDate is required")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    }
}