using AutoMapper;
using HomeManagement.Core.DTO;
using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Core.ViewModels;
using HomeManagement.Infrastructure.Database;
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
            var user = _userManager.Users.First(x => x.Email == dto.UserEmail);
            var entity = _mapper.Map<CalendarEvent>(dto);
            entity.UserId = user.Id;
            await _calendarEventRepo.AddAsync(entity);
            await _calendarEventRepo.SaveChangesAsync();
            return _mapper.Map<CalendarEventVM>((entity, user!.Email));
        }
    }
}
