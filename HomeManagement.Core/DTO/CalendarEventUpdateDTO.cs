namespace HomeManagement.Core.DTO
{
    public class CalendarEventUpdateDTO
    {
        public string Title { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string UserEmail { get; set; } = null!;
    }
}
