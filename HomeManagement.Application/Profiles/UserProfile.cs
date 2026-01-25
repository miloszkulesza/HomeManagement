using AutoMapper;
using HomeManagement.Application.DTO;
using HomeManagement.Application.ViewModels;
using HomeManagement.Core.Entities;

namespace HomeManagement.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<(User user, IList<string> userRoles), ApplicationUserVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.user.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email))
                .ForMember(dest => dest.CalendarEventBackgroundColor, opt => opt.MapFrom(src => src.user.CalendarEventBackgroundColor))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.userRoles));

            CreateMap<ApplicationUserUpdateDTO, User>()
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.WorkItems, opt => opt.Ignore())
                .ForMember(dest => dest.CalendarEvents, opt => opt.Ignore());
        }
    }
}
