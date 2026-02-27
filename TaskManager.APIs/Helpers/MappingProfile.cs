using AutoMapper;
using TaskManager.APIs.Dto.DashboardDtos;
using TaskManager.APIs.Dto.ProjectDtos;
using TaskManager.APIs.Dto.TaskDtos;
using TaskManager.Core.Entities.Dashboard;
using TaskManager.Core.Entities.Projects;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.APIs.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Task mappings
            CreateMap<AppTask, TaskReturnDto>()
                .ForMember(dest => dest.AssignedToName,
                    opt => opt.MapFrom(src => src.AssignedTo.DisplayName))
                .ForMember(dest => dest.CreatedByName,
                    opt => opt.MapFrom(src => src.CreatedBy.DisplayName))
                .ForMember(dest => dest.Team,
                    opt => opt.MapFrom(src => src.Team.ToString()))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateTaskDto, AppTask>();

            CreateMap<UpdateTaskDto, AppTask>();

            //-----------------------
            //Dashboard mappings
            CreateMap<DashboardSummaryResult, DashboardSummaryDto>();

            //-------------------------
            //Projects mappings
            CreateMap<CreateProjectDto, Project>();
            CreateMap<Project, ProjectReturnDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
