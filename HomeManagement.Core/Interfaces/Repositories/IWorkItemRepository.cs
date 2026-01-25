using HomeManagement.Core.Entities;

namespace HomeManagement.Core.Interfaces.Repositories
{
    public interface IWorkItemRepository : IRepository<WorkItem>
    {
        Task DeleteDoneWorkItems();
    }
}
