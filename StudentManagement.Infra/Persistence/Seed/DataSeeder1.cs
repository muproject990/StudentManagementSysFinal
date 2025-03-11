using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementSystem.Infrastructure.Persistence;

namespace StudentManagement.Infra.Persistence
{
    public static class DataSeeder1
    {
        public static void Seed(StudentDbContext1 context)
        {
            // Seed Courses first, so that they can be referenced
            if (!context.Courses.Any())
            {
                var courses = new List<Course>
                {
                    new Course { CourseName = "Calculus", CourseCode = "MATH101", CreditHours = 3 },
                    new Course { CourseName = "Physics", CourseCode = "PHYS101", CreditHours = 4 },
                    new Course { CourseName = "Chemistry", CourseCode = "CHEM101", CreditHours = 3 },
                    new Course { CourseName = "Literature", CourseCode = "ENG101", CreditHours = 2 },
                    new Course { CourseName = "Computer Science", CourseCode = "CS101", CreditHours = 4 }
                };

                context.Courses.AddRange(courses);
                context.SaveChanges();
            }

            // Seed Students (insert 20 sample students)
            if (!context.Students.Any())
            {
                var students = new List<Student>
                {
                    new Student { FullName = "Alice Johnson", DateOfBirth = new DateTime(2000, 1, 1), Email = "alice@example.com", PhoneNumber = "1234567890" },
                    new Student { FullName = "Bob Smith", DateOfBirth = new DateTime(1999, 2, 15), Email = "bob@example.com", PhoneNumber = "2345678901" },
                    new Student { FullName = "Charlie Brown", DateOfBirth = new DateTime(2001, 3, 10), Email = "charlie@example.com", PhoneNumber = "3456789012" },
                    new Student { FullName = "Diana Prince", DateOfBirth = new DateTime(2000, 4, 5), Email = "diana@example.com", PhoneNumber = "4567890123" },
                    new Student { FullName = "Edward Norton", DateOfBirth = new DateTime(1998, 5, 20), Email = "edward@example.com", PhoneNumber = "5678901234" },
                    new Student { FullName = "Fiona Gallagher", DateOfBirth = new DateTime(2002, 6, 18), Email = "fiona@example.com", PhoneNumber = "6789012345" },
                    new Student { FullName = "George Clooney", DateOfBirth = new DateTime(1997, 7, 22), Email = "george@example.com", PhoneNumber = "7890123456" },
                    new Student { FullName = "Hannah Baker", DateOfBirth = new DateTime(2001, 8, 9), Email = "hannah@example.com", PhoneNumber = "8901234567" },
                    new Student { FullName = "Ian Somerhalder", DateOfBirth = new DateTime(1998, 9, 14), Email = "ian@example.com", PhoneNumber = "9012345678" },
                    new Student { FullName = "Jane Doe", DateOfBirth = new DateTime(2000, 10, 31), Email = "jane@example.com", PhoneNumber = "1123456789" },
                    new Student { FullName = "Kevin Hart", DateOfBirth = new DateTime(1999, 11, 11), Email = "kevin@example.com", PhoneNumber = "2234567890" },
                    new Student { FullName = "Laura Palmer", DateOfBirth = new DateTime(2000, 12, 1), Email = "laura@example.com", PhoneNumber = "3345678901" },
                    new Student { FullName = "Michael Scott", DateOfBirth = new DateTime(1997, 1, 20), Email = "michael@example.com", PhoneNumber = "4456789012" },
                    new Student { FullName = "Nancy Drew", DateOfBirth = new DateTime(2001, 2, 28), Email = "nancy@example.com", PhoneNumber = "5567890123" },
                    new Student { FullName = "Oscar Wilde", DateOfBirth = new DateTime(1998, 3, 15), Email = "oscar@example.com", PhoneNumber = "6678901234" },
                    new Student { FullName = "Paula Abdul", DateOfBirth = new DateTime(1999, 5, 5), Email = "paula@example.com", PhoneNumber = "7789012345" },
                    new Student { FullName = "Quentin Tarantino", DateOfBirth = new DateTime(1997, 6, 30), Email = "quentin@example.com", PhoneNumber = "8890123456" },
                    new Student { FullName = "Rachel Green", DateOfBirth = new DateTime(2000, 7, 7), Email = "rachel@example.com", PhoneNumber = "9901234567" },
                    new Student { FullName = "Sam Winchester", DateOfBirth = new DateTime(1998, 8, 25), Email = "sam@example.com", PhoneNumber = "1012345678" },
                    new Student { FullName = "Tina Fey", DateOfBirth = new DateTime(1999, 9, 9), Email = "tina@example.com", PhoneNumber = "1212345678" }
                };

                context.Students.AddRange(students);
                context.SaveChanges();
            }

            // Seed Enrollments: Enroll each student in one or two random courses.
            if (!context.Enrollments.Any())
            {
                var random = new Random();
                // Get the list of existing students and courses.
                var allStudents = context.Students.ToList();
                var allCourses = context.Courses.ToList();
                var enrollments = new List<Enrollment>();

                // Enroll each student in one or two courses randomly.
                foreach (var student in allStudents)
                {
                    // Decide number of courses for this student (between 1 and 2)
                    int count = random.Next(1, 3);
                    // Shuffle course list and take distinct courses.
                    var coursesForStudent = allCourses.OrderBy(x => random.Next()).Take(count);

                    foreach (var course in coursesForStudent)
                    {
                        enrollments.Add(new Enrollment
                        {
                            StudentID = student.StudentID,
                            CourseID = course.CourseID,
                            EnrollmentDate = DateTime.UtcNow
                        });
                    }
                }

                context.Enrollments.AddRange(enrollments);
                context.SaveChanges();
            }

            // Seed Grades: Assign grades to some enrollments.
            if (!context.Grades.Any())
            {
                var random = new Random();
                var enrollmentList = context.Enrollments.ToList();
                var allowedGrades = new[] { 'A', 'B', 'C', 'D', 'F' };
                var grades = new List<Grade>();

                // For demonstration, assign a grade to roughly 75% of enrollments.
                foreach (var enrollment in enrollmentList)
                {
                    if (random.NextDouble() < 0.75) // 75% chance
                    {
                        grades.Add(new Grade
                        {
                            StudentID = enrollment.StudentID,
                            CourseID = enrollment.CourseID,
                            // Randomly pick one of the allowed grade letters.
                            GradeLetter = allowedGrades[random.Next(allowedGrades.Length)]
                        });
                    }
                }

                context.Grades.AddRange(grades);
                context.SaveChanges();
            }
        }
    }
}