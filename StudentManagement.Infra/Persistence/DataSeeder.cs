using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagement.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence
{
    public class DataSeeder
    {
        public static void Seed(StudentDbContext context)
        {
            // Seed Students if none exist
            if (!context.Students.Any())
            {
                var students = new List<Student>
                {
                    new() { FullName = "Alice Johnson", DateOfBirth = new DateTime(2000,1,1), Email = "alice@example.com", PhoneNumber = "1234567890" },
                    new() { FullName = "Bob Smith", DateOfBirth = new DateTime(1999,5,20), Email = "bob@example.com", PhoneNumber = "2345678901" },
                    new() { FullName = "Charlie Brown", DateOfBirth = new DateTime(2001,7,15), Email = "charlie@example.com", PhoneNumber = "3456789012" },
                    new() { FullName = "Diana Prince", DateOfBirth = new DateTime(2000,12,5), Email = "diana@example.com", PhoneNumber = "4567890123" },
                    new() { FullName = "Edward Norton", DateOfBirth = new DateTime(1998,3,30), Email = "edward@example.com", PhoneNumber = "5678901234" }
                };
                context.Students.AddRange(students);
            }

            // Seed Courses if none exist
            if (!context.Courses.Any())
            {
                var courses = new List<Course>
                {
                    new() { CourseName = "Calculus", CourseCode = "MATH101", CreditHours = 3 },
                    new() { CourseName = "Physics", CourseCode = "PHYS101", CreditHours = 4 },
                    new () { CourseName = "Chemistry", CourseCode = "CHEM101", CreditHours = 3 },
                    new() { CourseName = "Literature", CourseCode = "ENG101", CreditHours = 2 },
                    new() { CourseName = "Computer Science", CourseCode = "CS101", CreditHours = 4 }
                };
                context.Courses.AddRange(courses);
            }

            context.SaveChanges();
        }
    }
}
