using HomeManagement.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Infrastructure.Database
{
    public sealed class HomeManagementContext(DbContextOptions<HomeManagementContext> options) 
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("identity");

            modelBuilder.Entity<CalendarEvent>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.CalendarEvents)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
