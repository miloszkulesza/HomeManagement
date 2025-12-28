namespace HomeManagement.Core.DTO
{
    public class CalendarEventCreateDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public string UserEmail { get; set; } = null!;
    }
}
