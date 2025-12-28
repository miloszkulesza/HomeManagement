using AutoMapper;
using HomeManagement.Core.DTO;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Core.ViewModels;
using HomeManagement.Infrastructure.Migrations;

namespace HomeManagement.Infrastructure.Services
{
    public class CalendarEventService : ICalendarEventService
    {
        private readonly ICalendarEventRepository _calendarEventRepo;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;

        public CalendarEventService(ICalendarEventRepository calendarEventRepo,
            IMapper mapper,
            IAdminService adminService)
        {
            _calendarEventRepo = calendarEventRepo;
            _mapper = mapper;
            _adminService = adminService;
        }

        public async Task<List<CalendarEventVM>> GetCalendarEvents()
        {
            var calendarEvents = await _calendarEventRepo.GetAllAsync();
            List<CalendarEventVM> viewModels = new List<CalendarEventVM>();
            foreach (var calendarEvent in calendarEvents)
            {
                var user = await _adminService.GetUserById(calendarEvent.UserId);
                viewModels.Add(_mapper.Map<CalendarEventVM>((calendarEvent, user!.Email)));
            }
            return viewModels;
        }

        public async Task<CalendarEventVM> CreateCalendarEvent(CalendarEventCreateDTO dto)
        {
            var entity = _mapper.Map<CalendarEvent>(dto);
            await _calendarEventRepo.AddAsync(entity);
            await _calendarEventRepo.SaveChangesAsync();
            var user = await _adminService.GetUserById(entity.UserId);
            return _mapper.Map<CalendarEventVM>((entity, user!.Email));
        }
    }
}
