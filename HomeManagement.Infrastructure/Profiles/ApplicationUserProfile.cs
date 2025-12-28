using AutoMapper;
using HomeManagement.Core.ViewModels;
using HomeManagement.Infrastructure.Database;

namespace HomeManagement.Core.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<(ApplicationUser user, IList<string> userRoles), ApplicationUserVM>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email))
                .ForMember(dest => dest.CalendarEventBackgroundColor, opt => opt.MapFrom(src => src.user.CalendarEventBackgroundColor))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.userRoles));
        }
    }
}
