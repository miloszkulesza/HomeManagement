using HomeManagement.Core.Entities;

namespace HomeManagement.Core.Interfaces.Services
{
    public interface ICalendarEventService
    {
        Task<List<CalendarEvent>> GetCalendarEvents();
        Task<CalendarEvent> CreateCalendarEvent(CalendarEvent calendarEvent);
        Task RemoveCalendarEvent(string id);
        Task<CalendarEvent> UpdatePutCalendarEvent(CalendarEvent calendarEvent);
    }
}
