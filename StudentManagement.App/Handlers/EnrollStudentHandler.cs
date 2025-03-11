using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagement.App.Commands;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Errors;
using StudentManagementSystem.Infrastructure.Persistence;

namespace StudentManagement.App.Handlers
{
    public class EnrollStudentHandler
    {
        private readonly StudentDbContext _context;

        public EnrollStudentHandler(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<bool>> HandleAsync(EnrollStudentCommand command)
        {
            try
            {
                // Prevent duplicate enrollment
                bool alreadyEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.StudentID == command.StudentID && e.CourseID == command.CourseID);

                if (alreadyEnrolled)
                {
                    return ServiceResult<bool>.AsFailure("Student is already enrolled in this course.");
                }

                // Create a new enrollment record
                var enrollment = new Enrollment
                {
                    StudentID = command.StudentID,
                    CourseID = command.CourseID,
                    EnrollmentDate = DateTime.Now
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.AsSuccess(true);
            }
            catch (Exception ex)
            {
                // For production apps, log the detailed exception
                return ServiceResult<bool>.AsFailure($"Error enrolling student: {ex.Message}");
            }
        }
    }
}
