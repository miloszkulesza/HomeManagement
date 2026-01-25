using AutoMapper;
using HomeManagement.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeManagement.Infrastructure.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, IdentityRole>()
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            CreateMap<IdentityRole, Role>();
        }
    }
}
