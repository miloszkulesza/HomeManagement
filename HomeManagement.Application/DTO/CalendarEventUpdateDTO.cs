using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Application.DTO
{
    public class CalendarEventUpdateDTO
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
