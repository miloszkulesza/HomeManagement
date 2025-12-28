using HomeManagement.Core.Interfaces;

namespace HomeManagement.Core.ViewModels
{
    public class CalendarEventVM
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public DateTime? StartDate { get; set; }
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
    }
}
