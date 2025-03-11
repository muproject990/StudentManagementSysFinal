using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using StudentManagementSystem.Infrastructure.Persistence;
using System.IO;

namespace StudentManagement.Infra.Persistence
{
    public class StudentDbContext1Factory : IDesignTimeDbContextFactory<StudentDbContext1>
    {
        public StudentDbContext1 CreateDbContext(string[] args)
        {
            // Build configuration to get the connection string from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string for DbContext1
            var connectionString = configuration.GetConnectionString("StudentManagementContext1");

            // Use options builder to configure the DbContext
            var optionsBuilder = new DbContextOptionsBuilder<StudentDbContext1>();
            optionsBuilder.UseSqlite(connectionString);

            return new StudentDbContext1(optionsBuilder.Options);
        }
    }
}
