using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;

namespace HomeManagement.Application.Services
{
    public class WorkItemService(
        IWorkItemRepository workItemRepository,
        IIdentityService identityService) : IWorkItemService
    {
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;
        private readonly IIdentityService _identityService = identityService;

        public async Task<WorkItem> CreateWorkItem(WorkItem workItem)
        {
            await ValidateAssignee(workItem.UserId);
            await _workItemRepository.AddAsync(workItem);
            await _workItemRepository.SaveChangesAsync();
            return workItem;
        }

        public async Task<List<WorkItem>> GetWorkItems()
        {
            return (await _workItemRepository.GetAllAsync()).ToList();
        }

        public async Task RemoveWorkItem(Guid id)
        {
            var foundWorkItem = await GetWorkItemById(id);
            _workItemRepository.Remove(foundWorkItem);
            await _workItemRepository.SaveChangesAsync();
        }

        public async Task<WorkItem> UpdatePutWorkItem(Guid id, WorkItem workItem)
        {
            await ValidateAssignee(workItem.UserId);

            var existing = await GetWorkItemById(id);
            existing.Title = workItem.Title;
            existing.Priority = workItem.Priority;
            existing.IsDone = workItem.IsDone;
            existing.UserId = workItem.UserId;

            await _workItemRepository.SaveChangesAsync();
            return existing;
        }

        private async Task<WorkItem> GetWorkItemById(Guid id)
        {
            var workItem = await _workItemRepository.GetByIdAsync(id);
            if (workItem is null)
                throw new KeyNotFoundException($"Nie odnaleziono zadania o identyfikatorze {id}.");
            return workItem;
        }

        private async Task ValidateAssignee(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId) || await _identityService.GetUserById(userId) is null)
                throw new ArgumentException("Wskazany użytkownik nie istnieje.");
        }

        public async Task DeleteDoneWorkItems()
        {
            await _workItemRepository.DeleteDoneWorkItems();
        }
    }
}
