using AutoMapper;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Infrastructure.Database;
using HomeManagement.Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Infrastructure.Services
{
    public class CalendarEventService : ICalendarEventService
    {
        private readonly ICalendarEventRepository _calendarEventRepo;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CalendarEventService(ICalendarEventRepository calendarEventRepo,
            IMapper mapper,
            IAdminService adminService,
            UserManager<ApplicationUser> userManager)
        {
            _calendarEventRepo = calendarEventRepo;
            _mapper = mapper;
            _adminService = adminService;
            _userManager = userManager;
        }

        public async Task<List<CalendarEvent>> GetCalendarEvents()
        {
            var calendarEvents = await _calendarEventRepo.GetAllAsync();
            return calendarEvents.ToList();
        }

        public async Task<CalendarEvent> CreateCalendarEvent(CalendarEvent calendarEvent)
        {
            //var user = _userManager.Users.FirstOrDefault(x => x.Email == dto.UserEmail);
            //if (user is null)
            //    throw new Exception($"Nie znaleziono użytkownika o adresie email {dto.UserEmail}");
            //var entity = _mapper.Map<CalendarEvent>(dto);
            //entity.UserId = user.Id;
            //await _calendarEventRepo.AddAsync(entity);
            //await _calendarEventRepo.SaveChangesAsync();
            //return _mapper.Map<CalendarEventVM>((entity, user));
            await _calendarEventRepo.AddAsync(calendarEvent);
            await _calendarEventRepo.SaveChangesAsync();
            return calendarEvent;
        }

        public async Task RemoveCalendarEvent(string id)
        {
            var calendarEvent =  await  GetCalendarEventById(id);
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
            Guid guid;
            var parseResult = Guid.TryParse(id, out guid);
            if (!parseResult)
                throw new Exception($"Wartość {id} nie jest prawidłowa");
            var calendarEvent = await _calendarEventRepo.GetByIdAsync(guid);
            if (calendarEvent is null)
                throw new Exception($"Nie odnaleziono wydarzenia w kalendarzu o identyfikatorze {id}");
            return calendarEvent;
        }
    }
}
