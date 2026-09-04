using FluentAssertions;
using HomeManagement.Application.Services;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;

namespace HomeManagement.Tests;

public class WorkItemServiceTests
{
    [Fact]
    public async Task Create_rejects_unknown_assignee_without_saving()
    {
        var repository = new WorkItemRepositoryFake();
        var service = new WorkItemService(repository, new IdentityServiceFake());
        var workItem = new WorkItem
        {
            Id = Guid.NewGuid(),
            Title = "Zadanie",
            UserId = "missing-user"
        };

        var action = () => service.CreateWorkItem(workItem);

        await action.Should().ThrowAsync<ArgumentException>();
        repository.SaveCount.Should().Be(0);
    }

    [Fact]
    public async Task Update_changes_editable_fields_for_existing_task()
    {
        var existing = new WorkItem
        {
            Id = Guid.NewGuid(),
            Title = "Stare zadanie",
            UserId = "user-1"
        };
        var repository = new WorkItemRepositoryFake(existing);
        var identity = new IdentityServiceFake(new User { Id = "user-2", Email = "user@example.local" });
        var service = new WorkItemService(repository, identity);

        var updated = await service.UpdatePutWorkItem(existing.Id, new WorkItem
        {
            Title = "Nowe zadanie",
            Priority = true,
            IsDone = true,
            UserId = "user-2"
        });

        updated.Id.Should().Be(existing.Id);
        updated.Title.Should().Be("Nowe zadanie");
        updated.Priority.Should().BeTrue();
        updated.IsDone.Should().BeTrue();
        updated.UserId.Should().Be("user-2");
        repository.SaveCount.Should().Be(1);
    }

    private sealed class WorkItemRepositoryFake(params WorkItem[] items) : IWorkItemRepository
    {
        private readonly List<WorkItem> _items = [.. items];
        public int SaveCount { get; private set; }

        public Task<WorkItem?> GetByIdAsync(Guid id) =>
            Task.FromResult(_items.SingleOrDefault(item => item.Id == id));
        public Task<IEnumerable<WorkItem>> GetAllAsync() => Task.FromResult<IEnumerable<WorkItem>>(_items);
        public Task AddAsync(WorkItem entity)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }
        public void Update(WorkItem entity) { }
        public void Remove(WorkItem entity) => _items.Remove(entity);
        public Task SaveChangesAsync()
        {
            SaveCount++;
            return Task.CompletedTask;
        }
        public Task DeleteDoneWorkItems()
        {
            _items.RemoveAll(item => item.IsDone);
            return Task.CompletedTask;
        }
    }

    private sealed class IdentityServiceFake(params User[] users) : IIdentityService
    {
        public Task<User?> GetUserById(string id) =>
            Task.FromResult(users.SingleOrDefault(user => user.Id == id));
        public Task<User?> GetUserByEmail(string email) => throw new NotSupportedException();
        public Task<List<User>> GetUsers() => throw new NotSupportedException();
        public Task<IList<string>> GetUserRolesAsync(string userId) => throw new NotSupportedException();
        public Task<List<Role>> GetRoles() => throw new NotSupportedException();
        public Task UpdateUserAsync(string id, User user) => throw new NotSupportedException();
    }
}
