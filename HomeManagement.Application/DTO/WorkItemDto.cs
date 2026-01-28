namespace HomeManagement.Application.DTO
{
    public class WorkItemDto
    {
        public required string Title { get; set; }
        public bool Priority { get; set; }
        public bool IsDone { get; set; }
        public required string UserId { get; set; }
    }
}