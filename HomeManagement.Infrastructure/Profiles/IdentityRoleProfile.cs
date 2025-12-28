using AutoMapper;
using HomeManagement.Core.ViewModels;
using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Infrastructure.Profiles
{ 
    public class IdentityRoleProfile : Profile
    {
        public IdentityRoleProfile()
        {
            CreateMap<(IdentityRole role, IList<ApplicationUser> users), IdentityRoleVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.role.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.role.Name))
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.role.NormalizedName))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.users));
        }
    }
}
