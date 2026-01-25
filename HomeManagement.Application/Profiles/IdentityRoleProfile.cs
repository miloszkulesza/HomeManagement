using AutoMapper;
using HomeManagement.Application.ViewModels;
using HomeManagement.Core.Entities;

namespace HomeManagement.Application.Profiles
{ 
    public class IdentityRoleProfile : Profile
    {
        public IdentityRoleProfile()
        {
            CreateMap<(Role role, IList<User> users), IdentityRoleVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.role.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.role.Name))
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.role.NormalizedName))
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.users));
        }
    }
}
