using AutoMapper;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;

namespace HomeManagement.Application.Services
{
    public class CalendarEventService : ICalendarEventService
    {
        private readonly ICalendarEventRepository _calendarEventRepo;
        private readonly IMapper _mapper;

        public CalendarEventService(ICalendarEventRepository calendarEventRepo, IMapper mapper)
        {
            _calendarEventRepo = calendarEventRepo;
            _mapper = mapper;
        }

        public async Task<List<CalendarEvent>> GetCalendarEvents()
        {
            var calendarEvents = await _calendarEventRepo.GetAllAsync();
            return calendarEvents.ToList();
        }

        public async Task<CalendarEvent> CreateCalendarEvent(CalendarEvent calendarEvent)
        {
            await _calendarEventRepo.AddAsync(calendarEvent);
            await _calendarEventRepo.SaveChangesAsync();
            return calendarEvent;
        }

        public async Task RemoveCalendarEvent(string id)
        {
            var calendarEvent = await GetCalendarEventById(id);
            _calendarEventRepo.Remove(calendarEvent);
            await _calendarEventRepo.SaveChangesAsync();
        }

        public async Task<CalendarEvent> UpdatePutCalendarEvent(CalendarEvent calendarEvent)
        {
            _calendarEventRepo.Update(calendarEvent);
            await _calendarEventRepo.SaveChangesAsync();
            return calendarEvent;
        }

        private async Task<CalendarEvent> GetCalendarEventById(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                throw new Exception($"Wartoœæ {id} nie jest prawid³owa");
            var calendarEvent = await _calendarEventRepo.GetByIdAsync(guid);
            if (calendarEvent is null)
                throw new Exception($"Nie odnaleziono wydarzenia w kalendarzu o identyfikatorze {id}");
            return calendarEvent;
        }
    }
}