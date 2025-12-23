using HomeManagement.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Infrastructure.Database
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string CalendarEventBackgroundColor { get; set; } = null!;
        public ICollection<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();
    }
}
