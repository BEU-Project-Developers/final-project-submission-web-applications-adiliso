using Microsoft.EntityFrameworkCore;
using Construction.Models;

namespace Construction
{
    public class AppDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Service)
                .WithMany(s => s.Projects)
                .HasForeignKey(p => p.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Project)
                .WithMany(p => p.Photos)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}