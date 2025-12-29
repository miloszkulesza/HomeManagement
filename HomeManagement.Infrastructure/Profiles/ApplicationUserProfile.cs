using AutoMapper;
using HomeManagement.Core.DTO;
using HomeManagement.Core.ViewModels;
using HomeManagement.Infrastructure.Database;

namespace HomeManagement.Core.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<(ApplicationUser user, IList<string> userRoles), ApplicationUserVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.user.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email))
                .ForMember(dest => dest.CalendarEventBackgroundColor, opt => opt.MapFrom(src => src.user.CalendarEventBackgroundColor))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.userRoles));

            CreateMap<ApplicationUserUpdateDTO, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
                .ForMember(dest => dest.CalendarEvents, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore());
        }
    }
}
