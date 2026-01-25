using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Infrastructure.Repositories
{
    public class WorkItemRepository : Repository<WorkItem>, IWorkItemRepository
    {
        public WorkItemRepository(HomeManagementContext context) 
            : base(context)
        {

        }

        public async Task DeleteDoneWorkItems()
        {
            await _dbSet.Where(w => w.IsDone)
                .ExecuteDeleteAsync();
        }
    }
}
