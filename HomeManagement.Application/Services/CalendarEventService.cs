using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;

namespace HomeManagement.Application.Services
{
    public class CalendarEventService : ICalendarEventService
    {
        private readonly ICalendarEventRepository _calendarEventRepo;

        public CalendarEventService(ICalendarEventRepository calendarEventRepo)
        {
            _calendarEventRepo = calendarEventRepo;
        }

        public async Task<List<CalendarEvent>> GetCalendarEvents()
        {
            var calendarEvents = await _calendarEventRepo.GetAllAsync();
            return calendarEvents.ToList();
        }

        public async Task<CalendarEvent> CreateCalendarEvent(CalendarEvent calendarEvent)
        {
            ValidateDateRange(calendarEvent);
            await _calendarEventRepo.AddAsync(calendarEvent);
            await _calendarEventRepo.SaveChangesAsync();
            return calendarEvent;
        }

        public async Task RemoveCalendarEvent(Guid id)
        {
            var calendarEvent = await GetCalendarEventById(id);
            _calendarEventRepo.Remove(calendarEvent);
            await _calendarEventRepo.SaveChangesAsync();
        }

        public async Task<CalendarEvent> UpdatePutCalendarEvent(Guid id, CalendarEvent calendarEvent)
        {
            ValidateDateRange(calendarEvent);

            var existing = await GetCalendarEventById(id);
            existing.Title = calendarEvent.Title;
            existing.StartDate = calendarEvent.StartDate;
            existing.EndDate = calendarEvent.EndDate;

            await _calendarEventRepo.SaveChangesAsync();
            return existing;
        }

        private async Task<CalendarEvent> GetCalendarEventById(Guid id)
        {
            var calendarEvent = await _calendarEventRepo.GetByIdAsync(id);
            if (calendarEvent is null)
                throw new KeyNotFoundException($"Nie odnaleziono wydarzenia o identyfikatorze {id}.");
            return calendarEvent;
        }

        private static void ValidateDateRange(CalendarEvent calendarEvent)
        {
            if (calendarEvent.EndDate <= calendarEvent.StartDate)
                throw new ArgumentException("Data zakończenia musi być późniejsza niż data rozpoczęcia.");
        }
    }
}
