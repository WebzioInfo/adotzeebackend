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
        public DbSet<Review> Reviews { get; set; }
        public DbSet<AptitudeCategory> AptitudeCategories { get; set; }
        public DbSet<AptitudeQuestion> AptitudeQuestions { get; set; }
        public DbSet<AssessmentResult> AssessmentResults { get; set; }
        public DbSet<Scholarship> Scholarships { get; set; }
        public DbSet<ScholarshipEnquiry> ScholarshipEnquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Lead>()
                .HasIndex(l => l.Email);

            modelBuilder.Entity<Lead>()
                .HasIndex(l => l.AssignedToUserId);

            modelBuilder.Entity<Lead>()
                .HasIndex(l => l.Status);

            modelBuilder.Entity<Course>()
                .Property(c => c.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Course>()
                .Property(c => c.Stream)
                .HasConversion<string>();

            modelBuilder.Entity<Review>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Review>()
                .Property(r => r.VerificationType)
                .HasConversion<string>();

            modelBuilder.Entity<AddonCollege>()
                .HasOne(ac => ac.AddonCourse)
                .WithMany(a => a.AddonColleges)
                .HasForeignKey(ac => ac.AddonCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddonCollege>()
                .HasOne(ac => ac.College)
                .WithMany(c => c.AddonColleges)
                .HasForeignKey(ac => ac.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddonCourse>()
                .HasOne(ac => ac.Course)
                .WithMany(c => c.AddonCourses)
                .HasForeignKey(ac => ac.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Global Query Filters for Soft Delete
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Lead>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<College>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Course>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<AddonCourse>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<AddonCollege>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Review>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Scholarship>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<ScholarshipEnquiry>().HasQueryFilter(e => !e.IsDeleted);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
