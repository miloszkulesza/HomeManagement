using HomeManagement.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Infrastructure.Database
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string CalendarEventBackgroundColor { get; set; } = "#ffffff";
        public ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
        public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
    }
}
