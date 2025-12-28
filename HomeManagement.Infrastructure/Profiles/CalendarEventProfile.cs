using AutoMapper;
using HomeManagement.Core.DTO;
using HomeManagement.Core.Entities;
using HomeManagement.Core.ViewModels;

namespace HomeManagement.Core.Profiles
{
    public class CalendarEventProfile : Profile
    {
        public CalendarEventProfile()
        {
            CreateMap<(CalendarEvent calendarEvent, string userEmail), CalendarEventVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.calendarEvent.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.calendarEvent.UserId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.calendarEvent.StartDate))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.calendarEvent.Title))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.userEmail));
            CreateMap<CalendarEventCreateDTO, CalendarEvent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
