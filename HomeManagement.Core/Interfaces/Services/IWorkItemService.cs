using HomeManagement.Core.Entities;

namespace HomeManagement.Core.Interfaces.Services
{
    public interface IWorkItemService
    {
        Task<List<WorkItem>> GetWorkItems();
        Task<WorkItem> CreateWorkItem(WorkItem workItem);
        Task RemoveWorkItem(Guid id);
        Task<WorkItem> UpdatePutWorkItem(Guid id, WorkItem workItem);
        Task DeleteDoneWorkItems();
    }
}
