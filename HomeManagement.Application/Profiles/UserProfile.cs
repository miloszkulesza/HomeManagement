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
            CreateMap<ApplicationUserUpdateDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.CalendarEvents, opt => opt.Ignore())
                .ForMember(dest => dest.WorkItems, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<User, ApplicationUserVM>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));
        }
    }
}
