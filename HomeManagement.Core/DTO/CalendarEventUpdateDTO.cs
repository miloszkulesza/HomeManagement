namespace HomeManagement.Core.DTO
{
    public class CalendarEventUpdateDTO
    {
        public string Title { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserEmail { get; set; } = null!;
    }
}
