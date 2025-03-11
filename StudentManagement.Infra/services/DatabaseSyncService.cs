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

namespace StudentManagement.App.services
{
    public class DatabaseSynchronizationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseSynchronizationService> _logger;
        private readonly TimeSpan _syncInterval = TimeSpan.FromMinutes(60);

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
                await Task.Delay(_syncInterval, stoppingToken);
            }
        }

        /// <summary>
        /// Synchronizes the Students table from source context to destination context.
        /// Assumes that each Student record has a unique StudentID and uses LastModified
        /// to determine newer modifications.
        /// </summary>
        private async Task SynchronizeStudents(DbContext sourceContext, DbContext destinationContext)
        {
            // Load students from source and destination.
            var sourceStudents = await sourceContext.Set<Student>().AsNoTracking().ToListAsync();
            var destinationStudents = await destinationContext.Set<Student>().AsNoTracking().ToListAsync();

            // For each student in the source, check if it exists in the destination.
            foreach (var sourceStudent in sourceStudents)
            {
                var destStudent = destinationStudents.FirstOrDefault(s => s.StudentID == sourceStudent.StudentID);
                if (destStudent == null)
                {
                    // Insert new record
                    destinationContext.Set<Student>().Add(new Student
                    {
                        StudentID = sourceStudent.StudentID,
                        FullName = sourceStudent.FullName,
                        DateOfBirth = sourceStudent.DateOfBirth,
                        Email = sourceStudent.Email,
                        PhoneNumber = sourceStudent.PhoneNumber,
                        RegistrationDate = sourceStudent.RegistrationDate,
                        // Assuming LastModified is implemented:
                        LastModified = sourceStudent.LastModified
                    });
                }
                else
                {
                    // Update record if source is newer.
                    // For this example, we assume that a higher LastModified means newer.
                    if (sourceStudent.LastModified > destStudent.LastModified)
                    {
                        // Attach entity if not tracked and update fields.
                        destinationContext.Entry(destStudent).State = EntityState.Detached;
                        destinationContext.Set<Student>().Update(new Student
                        {
                            StudentID = sourceStudent.StudentID,
                            FullName = sourceStudent.FullName,
                            DateOfBirth = sourceStudent.DateOfBirth,
                            Email = sourceStudent.Email,
                            PhoneNumber = sourceStudent.PhoneNumber,
                            RegistrationDate = sourceStudent.RegistrationDate,
                            LastModified = sourceStudent.LastModified
                        });
                    }
                }
            }
            await destinationContext.SaveChangesAsync();
        }
    }
}