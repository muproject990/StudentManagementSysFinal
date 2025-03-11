using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementSystem.Infrastructure.Persistence;

namespace StudentManagement.Infra
{
    public static class InfrastructureExtensions
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString1 = configuration.GetConnectionString("StudentManagementContext1");
            var connectionString2 = configuration.GetConnectionString("StudentManagementContext2");

            services.AddDbContext<StudentDbContext1>(options =>
                options.UseSqlite(connectionString1));
            services.AddDbContext<StudentDbContext2>(options =>
            options.UseSqlite(connectionString2));


            return services;
        }
    }
}