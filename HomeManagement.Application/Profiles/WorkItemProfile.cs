using AutoMapper;
using HomeManagement.Application.DTO;
using HomeManagement.Application.ViewModels;
using HomeManagement.Core.Entities;

namespace HomeManagement.Application.Profiles
{
    public class WorkItemProfile : Profile
    {
        public WorkItemProfile()
        {
            CreateMap<WorkItemDto, WorkItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.AssignedToUserId));

            CreateMap<WorkItem, WorkItemDto>()
                .ForMember(dest => dest.AssignedToUserId, opt => opt.MapFrom(src => src.UserId));
            CreateMap<WorkItem, WorkItemVM>()
                .ForMember(dest => dest.AssignedToUserId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
