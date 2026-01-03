namespace HomeManagement.Core.DTO
{
    public class CalendarEventCreateDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string UserEmail { get; set; } = null!;
    }
}
