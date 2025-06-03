using Microsoft.EntityFrameworkCore;
using Construction.Models;

namespace Construction
{
    public class AppDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photo { get; set; }  // ✅ Add Photos DbSet

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Project - Service (many-to-one)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Service)
                .WithMany(s => s.Projects)
                .HasForeignKey(p => p.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Photo - Project (many-to-one)
            modelBuilder.Entity<Photo>()
                .HasOne(ph => ph.Project)
                .WithMany(pr => pr.Photos)
                .HasForeignKey(ph => ph.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Optional: Configure other constraints (like unique constraints, lengths etc.)
        }
    }
}