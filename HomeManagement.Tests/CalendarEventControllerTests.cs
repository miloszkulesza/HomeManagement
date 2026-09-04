using AutoMapper;
using FluentAssertions;
using HomeManagement.Application.Profiles;
using HomeManagement.Application.ViewModels;
using HomeManagement.Controllers;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeManagement.Tests;

public class CalendarEventControllerTests
{
    [Fact]
    public async Task GetCalendarEvents_maps_multiple_events_after_loading_users_once()
    {
        var user = new User
        {
            Id = "user-id",
            Email = "user@example.local",
            CalendarEventBackgroundColor = "#123456"
        };
        var events = Enumerable.Range(1, 20)
            .Select(index => new CalendarEvent
            {
                Id = Guid.NewGuid(),
                Title = $"Wydarzenie {index}",
                StartDate = DateTimeOffset.Parse("2030-01-01T10:00:00+00:00"),
                EndDate = DateTimeOffset.Parse("2030-01-01T11:00:00+00:00"),
                UserId = user.Id
            })
            .ToList();
        var adminService = new AdminServiceFake([user]);
        var controller = new CalendarEventController(
            CreateMapper(),
            new CalendarEventServiceFake(events),
            adminService);

        var response = await controller.GetCalendarEvents();

        var ok = response.Result.Should().BeOfType<OkObjectResult>().Subject;
        var models = ok.Value.Should().BeAssignableTo<List<CalendarEventVM>>().Subject;
        models.Should().HaveCount(20);
        models.Should().OnlyContain(model =>
            model.UserEmail == user.Email &&
            model.CalendarEventBackgroundColor == user.CalendarEventBackgroundColor);
        adminService.GetUsersCallCount.Should().Be(1);
        adminService.GetUserByIdCallCount.Should().Be(0);
    }

    private static IMapper CreateMapper()
    {
        var loggerFactory = LoggerFactory.Create(_ => { });
        var configuration = new MapperConfiguration(
            config => config.AddProfile<CalendarEventProfile>(),
            loggerFactory);
        return configuration.CreateMapper();
    }

    private sealed class CalendarEventServiceFake(List<CalendarEvent> events) : ICalendarEventService
    {
        public Task<List<CalendarEvent>> GetCalendarEvents() => Task.FromResult(events);
        public Task<CalendarEvent> CreateCalendarEvent(CalendarEvent calendarEvent) => throw new NotSupportedException();
        public Task RemoveCalendarEvent(Guid id) => throw new NotSupportedException();
        public Task<CalendarEvent> UpdatePutCalendarEvent(Guid id, CalendarEvent calendarEvent) =>
            throw new NotSupportedException();
    }

    private sealed class AdminServiceFake(List<User> users) : IAdminService
    {
        public int GetUsersCallCount { get; private set; }
        public int GetUserByIdCallCount { get; private set; }

        public Task<List<User>> GetUsers()
        {
            GetUsersCallCount++;
            return Task.FromResult(users);
        }

        public Task<User?> GetUserById(string id)
        {
            GetUserByIdCallCount++;
            return Task.FromResult(users.SingleOrDefault(user => user.Id == id));
        }

        public Task<User?> GetUser(string email) => throw new NotSupportedException();
        public Task<List<Role>> GetRoles() => throw new NotSupportedException();
        public Task<User> UpdatePutUserProfile(string id, User user) => throw new NotSupportedException();
    }
}
