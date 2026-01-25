namespace HomeManagement.Core.Entities
{
    public class User
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string CalendarEventBackgroundColor { get; set; } = "#ffffff";
        public List<CalendarEvent> CalendarEvents { get; set; } = new();
        public List<WorkItem> WorkItems { get; set; } = new();
        public List<string> Roles { get; set; } = new();
    }
}
