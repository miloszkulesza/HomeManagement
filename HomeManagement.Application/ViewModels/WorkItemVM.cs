namespace HomeManagement.Application.ViewModels
{
    public class WorkItemVM
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public bool Priority { get; set; }
        public bool IsDone { get; set; }
        public required string UserId { get; set; }
    }
}