using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Core.Entities
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        public required string Title { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public required string UserId { get; set; }
    }
}
