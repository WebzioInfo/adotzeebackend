using Adotzee_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Adotzee_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<AddonCourse> AddonCourses { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<AddonCollege> AddonColleges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Lead> Leads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(c => c.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Course>()
                .Property(c => c.Stream)
                .HasConversion<string>();

            modelBuilder.Entity<AddonCollege>()
                .HasOne(ac => ac.AddonCourse)
                .WithMany(a => a.AddonColleges)
                .HasForeignKey(ac => ac.AddonCourseId);

            modelBuilder.Entity<AddonCollege>()
                .HasOne(ac => ac.College)
                .WithMany(c => c.AddonColleges)
                .HasForeignKey(ac => ac.CollegeId);

            modelBuilder.Entity<AddonCourse>()
                .HasOne(ac => ac.Course)
                .WithMany(c => c.AddonCourses)
                .HasForeignKey(ac => ac.CourseId);

        }
    }
}
