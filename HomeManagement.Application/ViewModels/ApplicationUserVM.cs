namespace HomeManagement.Application.ViewModels
{
    public class ApplicationUserVM
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? CalendarEventBackgroundColor { get; set; }
        public List<string>? Roles { get; set; }
    }
}
