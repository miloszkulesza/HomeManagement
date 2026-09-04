using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Core.Entities
{
    public class WorkItem
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        public required string Title { get; set; }

        public bool Priority { get; set; }
        public bool IsDone { get; set; }
        public required string UserId { get; set; }
    }
}
