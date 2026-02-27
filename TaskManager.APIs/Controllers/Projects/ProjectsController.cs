using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.APIs.Dto.ProjectDtos;
using TaskManager.APIs.Dto.TaskDtos;
using TaskManager.APIs.Errors;
using TaskManager.Core.Entities.Projects;
using TaskManager.Core.Services.Contract;

namespace TaskManager.APIs.Controllers.Projects
{
    [Authorize(Roles = "Admin,TeamLeader")]
    public class ProjectsController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        // POST /api/projects
        // Admin/TeamLeader - create new project quote
        [HttpPost]
        public async Task<ActionResult<ProjectReturnDto>> CreateProject(CreateProjectDto dto)
        {
            var project = _mapper.Map<Project>(dto);
            var created = await _projectService.CreateAsync(project);
            return StatusCode(201, _mapper.Map<ProjectReturnDto>(created));
        }

        // PUT /api/projects/{id}/costs
        // Admin/TeamLeader - update costs, auto recalculates TotalPrice
        [HttpPut("{id}/costs")]
        public async Task<ActionResult<ProjectReturnDto>> UpdateCosts(
            int id, UpdateCostsDto dto)
        {
            var result = await _projectService.UpdateCostsAsync(
                id, dto.DesignCost, dto.ProductionCost);

            if (result is null)
                return NotFound(new ApiResponse(404, "Project not found"));

            return Ok(_mapper.Map<ProjectReturnDto>(result));
        }

        // PUT /api/projects/{id}/send
        // Admin/TeamLeader - change status to Sent
        [HttpPut("{id}/send")]
        public async Task<ActionResult<ProjectReturnDto>> ChangeStatusToSent(int id)
        {
            var result = await _projectService.ChangeStatusToSentAsync(id);

            if (result is null)
                return NotFound(new ApiResponse(404, "Project not found"));

            return Ok(_mapper.Map<ProjectReturnDto>(result));
        }
    }
}
