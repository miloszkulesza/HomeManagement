using AutoMapper;
using HomeManagement.Application.DTO;
using HomeManagement.Application.ViewModels;
using HomeManagement.Core.Entities;

namespace HomeManagement.Application.Profiles
{
    public class CalendarEventProfile : Profile
    {
        public CalendarEventProfile()
        {
            CreateMap<(CalendarEvent calendarEvent, User user), CalendarEventVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.calendarEvent.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.calendarEvent.UserId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.calendarEvent.StartDate))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.calendarEvent.Title))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.user.Email))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.calendarEvent.EndDate))
                .ForMember(dest => dest.CalendarEventBackgroundColor, opt => opt.MapFrom(src => src.user.CalendarEventBackgroundColor));

            CreateMap<CalendarEvent, CalendarEventVM>()
                .ForMember(dest => dest.UserEmail, opt => opt.Ignore())
                .ForMember(dest => dest.CalendarEventBackgroundColor, opt => opt.Ignore());

            CreateMap<CalendarEventCreateDTO, CalendarEvent>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<CalendarEventUpdateDTO, CalendarEvent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
