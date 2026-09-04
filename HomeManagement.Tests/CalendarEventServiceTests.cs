using FluentAssertions;
using HomeManagement.Application.Services;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;

namespace HomeManagement.Tests;

public class CalendarEventServiceTests
{
    [Fact]
    public async Task Update_preserves_author_and_changes_editable_fields()
    {
        var id = Guid.NewGuid();
        var existing = new CalendarEvent
        {
            Id = id,
            Title = "Stary tytuł",
            StartDate = DateTimeOffset.Parse("2026-09-04T10:00:00+02:00"),
            EndDate = DateTimeOffset.Parse("2026-09-04T11:00:00+02:00"),
            UserId = "author-id"
        };
        var repository = new CalendarEventRepositoryFake(existing);
        var service = new CalendarEventService(repository);
        var update = new CalendarEvent
        {
            Title = "Nowy tytuł",
            StartDate = existing.StartDate.AddHours(1),
            EndDate = existing.EndDate.AddHours(2),
            UserId = "ignored"
        };

        var result = await service.UpdatePutCalendarEvent(id, update);

        result.Should().BeSameAs(existing);
        result.Title.Should().Be("Nowy tytuł");
        result.UserId.Should().Be("author-id");
        repository.SaveCount.Should().Be(1);
    }

    [Fact]
    public async Task Create_rejects_an_invalid_date_range()
    {
        var repository = new CalendarEventRepositoryFake();
        var service = new CalendarEventService(repository);
        var calendarEvent = new CalendarEvent
        {
            Title = "Nieprawidłowe wydarzenie",
            StartDate = DateTimeOffset.Parse("2026-09-04T11:00:00+02:00"),
            EndDate = DateTimeOffset.Parse("2026-09-04T10:00:00+02:00"),
            UserId = "author-id"
        };

        var action = () => service.CreateCalendarEvent(calendarEvent);

        await action.Should().ThrowAsync<ArgumentException>();
        repository.SaveCount.Should().Be(0);
    }

    private sealed class CalendarEventRepositoryFake(params CalendarEvent[] events) : ICalendarEventRepository
    {
        private readonly List<CalendarEvent> _events = [.. events];

        public int SaveCount { get; private set; }

        public Task<CalendarEvent?> GetByIdAsync(Guid id) =>
            Task.FromResult(_events.SingleOrDefault(calendarEvent => calendarEvent.Id == id));

        public Task<IEnumerable<CalendarEvent>> GetAllAsync() =>
            Task.FromResult<IEnumerable<CalendarEvent>>(_events);

        public Task AddAsync(CalendarEvent entity)
        {
            _events.Add(entity);
            return Task.CompletedTask;
        }

        public void Update(CalendarEvent entity)
        {
        }

        public void Remove(CalendarEvent entity) => _events.Remove(entity);

        public Task SaveChangesAsync()
        {
            SaveCount++;
            return Task.CompletedTask;
        }
    }
}
