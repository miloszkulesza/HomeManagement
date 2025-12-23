using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Core.ViewModels;

namespace HomeManagement.Infrastructure.Services
{
    public class CalendarEventService : ICalendarEventService
    {
        private readonly ICalendarEventRepository _calendarEventRepo;

        public CalendarEventService(ICalendarEventRepository calendarEventRepo)
        {
            _calendarEventRepo = calendarEventRepo;
        }

        public async Task<List<CalendarEventVM>> GetCalendarEvents()
        {
            var calendarEvents = await _calendarEventRepo.GetAllAsync();
            List<CalendarEventVM> viewModels = calendarEvents.Select(x => new CalendarEventVM
            {
                Id = x.Id,
                StartDate = x.StartDate,
                Title = x.Title,
                UserId = x.UserId
            }).ToList();
            return viewModels;
        }
    }
}
