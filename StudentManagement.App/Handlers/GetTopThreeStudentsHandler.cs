using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Infrastructure.Persistence;
using StudentManagement.Domain.Errors;
using StudentManagement.Domain.Entities;
using StudentManagement.App.Queries;

namespace StudentManagementSystem.Application.Handlers
{
    public class GetTopThreeStudentsHandler(StudentDbContext1 context)
    {
        private readonly StudentDbContext1 _context = context;

        public async Task<ServiceResult<IEnumerable<Student>>> HandleAsync(GetTopThreeStudentsQuery query)
        {
            try
            {
                // Retrieve all students including their grades
                var students = await _context.Students
                    .Include(s => s.Grades)
                    .ToListAsync();

                // Calculate GPA for each student.
                // GPA calculation: average of numeric values mapped from GradeLetter;
                // if no grades exist, GPA is 0.
                var studentsWithGpa = students.Select(student => new
                {
                    Student = student,
                    Gpa = student.Grades.Count != 0
                           ? student.Grades.Average(g => g.GradeLetter switch
                           {
                               'A' => 4.0,
                               'B' => 3.0,
                               'C' => 2.0,
                               'D' => 1.0,
                               'F' => 0.0,
                               _ => 0.0
                           })
                           : 0.0
                });

                // Order students by descending GPA and take the top 3
                var topThree = studentsWithGpa
                    .OrderByDescending(x => x.Gpa)
                    .Take(3)
                    .Select(x => x.Student)
                    .ToList();

                return ServiceResult<IEnumerable<Student>>.AsSuccess(topThree);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<Student>>.AsFailure(ex.Message);
            }
        }
    }
}