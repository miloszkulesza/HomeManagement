using HomeManagement.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Infrastructure.Database
{
    public sealed class HomeManagementContext(DbContextOptions<HomeManagementContext> options) 
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CalendarEvent>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.CalendarEvents)
                .HasForeignKey(c => c.UserId)
                .IsRequired();

            modelBuilder.Entity<WorkItem>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.WorkItems)
                .HasForeignKey(w => w.UserId)
                .IsRequired();
        }
    }
}
