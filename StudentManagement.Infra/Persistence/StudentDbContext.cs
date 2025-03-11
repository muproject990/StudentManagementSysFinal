using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Entities;

namespace StudentManagementSystem.Infrastructure.Persistence
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(s => s.Email).IsUnique();
            });

            // Configure Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(c => c.CourseCode).IsUnique();
                entity.HasAnnotation("Sqlite:CheckConstraint",
                   "CK_Course_CreditHours CHECK (CreditHours IN(1,2,3,4,5))");
            });

            // Configure Enrollment entity to prevent a student enrolling twice in the same course.
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasIndex(e => new { e.StudentID, e.CourseID }).IsUnique();

                // Explicitly provide the generic type parameter for HasOne and WithMany:
                entity.HasOne(e => e.Student)  //  is explicit
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentID);

                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.CourseID);
            });

            // Configure Grade entity and its relationships, using HasAnnotation for SQLite check constraint:
            modelBuilder.Entity<Grade>(entity =>
            {
                // entity.HasAnnotation("Sqlite:CheckConstraint",
                //     "CK_Grades_GradeLetter CHECK (GradeLetter IN('A','B','C','D','F'))");

                entity.HasAnnotation("Sqlite:CheckConstraint", "CK_Grades_GradeLetter CHECK (GradeLetter IN('A', 'B', 'C', 'D', 'F'))");

                entity.HasOne(g => g.Student)
                      .WithMany(s => s.Grades)
                      .HasForeignKey(g => g.StudentID);

                entity.HasOne(g => g.Course)
                      .WithMany(c => c.Grades)
                      .HasForeignKey(g => g.CourseID);
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
