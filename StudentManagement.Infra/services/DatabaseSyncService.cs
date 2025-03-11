using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudentManagement.Domain.Entities;
using StudentManagementSystem.Infrastructure.Persistence;

namespace StudentManagement.Infra.services
{
    public class DatabaseSynchronizationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseSynchronizationService> _logger;

        public DatabaseSynchronizationService(IServiceProvider serviceProvider, ILogger<DatabaseSynchronizationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Database synchronization service is running.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting synchronization at {time}", DateTime.UtcNow);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var db1 = scope.ServiceProvider.GetRequiredService<StudentDbContext1>();
                        var db2 = scope.ServiceProvider.GetRequiredService<StudentDbContext2>();

                        // Synchronize Students from db1 to db2
                        await SynchronizeStudents(db1, db2);
                        // And synchronize Students from db2 to db1 for bidirectionality.
                        await SynchronizeStudents(db2, db1);
                    }

                    _logger.LogInformation("Synchronization complete at {time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during database synchronization.");
                }

                // Wait for the defined interval before next sync.
                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }

        // New method to allow manual sync triggering
        public async Task RunSyncAsync()
        {
            try
            {
                _logger.LogInformation("Manual synchronization started.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var db1 = scope.ServiceProvider.GetRequiredService<StudentDbContext1>();
                    var db2 = scope.ServiceProvider.GetRequiredService<StudentDbContext2>();

                    // Synchronize Students from db1 to db2
                    await SynchronizeStudents(db1, db2);
                    // And synchronize Students from db2 to db1 for bidirectionality.
                    await SynchronizeStudents(db2, db1);
                }

                _logger.LogInformation("Manual synchronization complete.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during manual synchronization.");
            }
        }

        private async Task SynchronizeStudents(DbContext sourceContext, DbContext destinationContext)
        {
            // Step 1: Retrieve students from source and destination
            var sourceStudents = await ReadStudents(sourceContext);
            var destinationStudents = await ReadStudents(destinationContext);

            // Step 2: Insert or update students in destination based on source data
            foreach (var sourceStudent in sourceStudents)
            {
                var destStudent = destinationStudents.FirstOrDefault(s => s.StudentID == sourceStudent.StudentID);

                if (destStudent == null)
                {
                    // Create: Insert new student if it does not exist in destination
                    await CreateStudent(destinationContext, sourceStudent);
                }
                else
                {
                    // Update: Update the student if source data is newer
                    if (sourceStudent.LastModified > destStudent.LastModified)
                    {
                        await UpdateStudent(destinationContext, sourceStudent);
                    }
                }
            }

            // Step 3: Delete students from destination that no longer exist in source
            var studentsToDelete = destinationStudents
                .Where(destStudent => !sourceStudents.Any(srcStudent => srcStudent.StudentID == destStudent.StudentID))
                .ToList();

            foreach (var studentToDelete in studentsToDelete)
            {
                await DeleteStudent(destinationContext, studentToDelete);
            }

            // Step 4: Save changes to the destination database (insert, update, delete)
            await destinationContext.SaveChangesAsync();
        }

        // Read students from a context
        private async Task<List<Student>> ReadStudents(DbContext context)
        {
            return await context.Set<Student>().AsNoTracking().ToListAsync();
        }

        // Create a student in the destination database
        private async Task CreateStudent(DbContext destinationContext, Student student)
        {
            destinationContext.Set<Student>().Add(new Student
            {
                StudentID = student.StudentID,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                RegistrationDate = student.RegistrationDate,
                LastModified = student.LastModified
            });
        }

        // Update an existing student in the destination database
        private async Task UpdateStudent(DbContext destinationContext, Student student)
        {
            // Detach the entity if it is being tracked, then update it
            var existingStudent = await destinationContext.Set<Student>().FindAsync(student.StudentID);
            if (existingStudent != null)
            {
                destinationContext.Entry(existingStudent).State = EntityState.Detached;
            }

            destinationContext.Set<Student>().Update(new Student
            {
                StudentID = student.StudentID,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                RegistrationDate = student.RegistrationDate,
                LastModified = student.LastModified
            });
        }

        // Delete a student from the destination database
        private async Task DeleteStudent(DbContext destinationContext, Student student)
        {
            destinationContext.Set<Student>().Remove(student);
        }

    }


}