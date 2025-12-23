namespace HomeManagement.Core.Entities
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public string UserId { get; set; } = null!;
    }
}
