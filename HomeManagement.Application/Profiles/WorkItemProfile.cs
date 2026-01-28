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
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<WorkItem, WorkItemDto>();
            CreateMap<WorkItem, WorkItemVM>();
        }
    }
}