using AutoMapper;
using HomeManagement.Core.Entities;
using HomeManagement.Infrastructure.Database;

namespace HomeManagement.Infrastructure.Profiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, User>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

        }
    }
}
