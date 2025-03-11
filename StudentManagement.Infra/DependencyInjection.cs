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
            var connectionString = configuration.GetConnectionString("StudentManagementContext");

            services.AddDbContext<StudentDbContext>(options =>
                options.UseSqlite(connectionString));


            return services;
        }
    }
}