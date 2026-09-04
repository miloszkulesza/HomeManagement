using HomeManagement.Core.Entities;

namespace HomeManagement.Core.Interfaces.Services
{
    public interface ICalendarEventService
    {
        Task<List<CalendarEvent>> GetCalendarEvents();
        Task<CalendarEvent> CreateCalendarEvent(CalendarEvent calendarEvent);
        Task RemoveCalendarEvent(Guid id);
        Task<CalendarEvent> UpdatePutCalendarEvent(Guid id, CalendarEvent calendarEvent);
    }
}
