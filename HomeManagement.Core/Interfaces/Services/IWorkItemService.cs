using HomeManagement.Core.Entities;

namespace HomeManagement.Core.Interfaces.Services
{
    public interface IWorkItemService
    {
        Task<List<WorkItem>> GetWorkItems();
        Task<WorkItem> CreateWorkItem(WorkItem workItem);
        Task RemoveWorkItem(string id);
        Task<WorkItem> UpdatePutWorkItem(WorkItem workItem);
        Task DeleteDoneWorkItems();
    }
}
