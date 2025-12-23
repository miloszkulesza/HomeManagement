using HomeManagement.Core.ViewModels;

namespace HomeManagement.Core.Interfaces.Services
{
    public interface ICalendarEventService
    {
        Task<List<CalendarEventVM>> GetCalendarEvents();
    }
}
