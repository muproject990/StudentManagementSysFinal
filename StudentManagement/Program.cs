using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentManagement.Infra;//   extension method AddInfrastructure
using StudentManagementSystem.Infrastructure.Persistence;
using StudentManagement.Domain.Interface;
using StudentManagement.Infra.Repository;
using StudentManagement.App.services;
using Microsoft.EntityFrameworkCore;
using StudentManagement.App.Automapper;
using StudentManagementSystem.Application.Handlers;
using StudentManagement.Infra.Persistence;
using StudentManagement.Infra.services;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Infrastructure & Services
// Register   custom AddInfrastructure extension,
// which registers   DbContext (using the connection string from configuration)
builder.Services.AddInfrastructure(builder.Configuration);

// Register generic repository and service for all entities.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));


// builder.Services.AddScoped<GetTopThreeStudentsHandler>();

// Register Mapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register controllers.
builder.Services.AddControllers();

// 2. Configure Swagger with JWT Authorization
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Define the Bearer scheme that's used to secure the endpoints.
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter   JWT token with 'Bearer ' prefix. Example: 'Bearer eyJhbGci...'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add a global security requirement for all operations.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 3. Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings")!;
string secretKey = jwtSettings.GetValue<string>("SecretKey")!;
string issuer = jwtSettings.GetValue<string>("Issuer")!;
string audience = jwtSettings.GetValue<string>("Audience")!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;  // set true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,  // ensure token hasn't expired
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


// 5. Registering the background services
builder.Services.AddHostedService<DatabaseSynchronizationService>();



var app = builder.Build();


// 4
using (var scope = app.Services.CreateScope())
{
    var db1 = scope.ServiceProvider.GetRequiredService<StudentDbContext1>();
    var db2 = scope.ServiceProvider.GetRequiredService<StudentDbContext2>();

    // Migrate both DbContexts
    db1.Database.Migrate();
    db2.Database.Migrate();
}



// 6. Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANT: Authentication must be added before Authorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();