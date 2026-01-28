using AutoMapper;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;

namespace HomeManagement.Application.Services
{
    public class WorkItemService(IWorkItemRepository _workItemRepository) : IWorkItemService
    {
        public async Task<WorkItem> CreateWorkItem(WorkItem workItem)
        {
            await _workItemRepository.AddAsync(workItem);
            await _workItemRepository.SaveChangesAsync();
            return workItem;
        }

        public async Task<List<WorkItem>> GetWorkItems()
        {
            return (await _workItemRepository.GetAllAsync()).ToList();
        }

        public async Task RemoveWorkItem(string id)
        {
            var foundWorkItem = await GetWorkItemById(id);
            _workItemRepository.Remove(foundWorkItem);
            await _workItemRepository.SaveChangesAsync();
        }

        public async Task<WorkItem> UpdatePutWorkItem(WorkItem workItem)
        {
            _workItemRepository.Update(workItem);
            await _workItemRepository.SaveChangesAsync();
            return workItem;
        }

        private async Task<WorkItem> GetWorkItemById(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                throw new Exception($"Wartość {id} nie jest prawidłowa");
            var workItem = await _workItemRepository.GetByIdAsync(guid);
            if (workItem is null)
                throw new Exception($"Nie odnaleziono zadania o identyfikatorze {id}");
            return workItem;
        }

        public async Task DeleteDoneWorkItems()
        {
            await _workItemRepository.DeleteDoneWorkItems();
        }
    }
}
