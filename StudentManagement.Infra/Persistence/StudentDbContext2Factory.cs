using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using StudentManagementSystem.Infrastructure.Persistence;
using System.IO;

namespace StudentManagement.Infra.Persistence
{
    public class StudentDbContext2Factory : IDesignTimeDbContextFactory<StudentDbContext2>
    {
        public StudentDbContext2 CreateDbContext(string[] args)
        {
            // Build configuration to get the connection string from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string for DbContext2
            var connectionString = configuration.GetConnectionString("StudentManagementContext2");

            // Use options builder to configure the DbContext
            var optionsBuilder = new DbContextOptionsBuilder<StudentDbContext2>();
            optionsBuilder.UseSqlite(connectionString);

            return new StudentDbContext2(optionsBuilder.Options);
        }
    }
}