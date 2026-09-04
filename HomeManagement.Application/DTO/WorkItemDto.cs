using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Application.DTO
{
    public class WorkItemDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public bool Priority { get; set; }
        public bool IsDone { get; set; }
        public string AssignedToUserId { get; set; } = string.Empty;
    }
}
