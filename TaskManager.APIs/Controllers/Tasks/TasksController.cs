using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.APIs.Dto.TaskDtos;
using TaskManager.APIs.Errors;
using TaskManager.Core.Entities.Identity;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Services.Contract;

namespace TaskManager.APIs.Controllers.Tasks
{
    [Authorize]
    public class TasksController : BaseController
    {

        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(
            ITaskService taskService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _mapper = mapper;
            _userManager = userManager;
        }

        // -------------------------------------------------------
        // POST /api/tasks
        // TeamLeader only - create a new task
        // -------------------------------------------------------
        [HttpPost]
        [Authorize(Roles = "TeamLeader")]
        public async Task<ActionResult<TaskReturnDto>> CreateTask(CreateTaskDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
                return Unauthorized(new ApiResponse(401));

            var task = _mapper.Map<AppTask>(dto);
            task.CreatedByUserId = currentUser.Id;

            var created = await _taskService.CreateTaskAsync(task);
            return StatusCode(201, _mapper.Map<TaskReturnDto>(created));
        }

        // -------------------------------------------------------
        // PUT /api/tasks/{id}
        // TeamLeader only - update task (only creator can update)
        // -------------------------------------------------------
        [HttpPut("{id}")]
        [Authorize(Roles = "TeamLeader")]
        public async Task<ActionResult<TaskReturnDto>> UpdateTask(int id, UpdateTaskDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
                return Unauthorized(new ApiResponse(401));

            var updatedTask = _mapper.Map<AppTask>(dto);
            var result = await _taskService.UpdateTaskAsync(id, updatedTask, currentUser.Id);

            if (result is null)
                return NotFound(new ApiResponse(404, "Task not found or you are not the creator"));

            return Ok(_mapper.Map<TaskReturnDto>(result));
        }

        // -------------------------------------------------------
        // PATCH /api/tasks/{id}/status
        // TeamMember only - update task status only
        // -------------------------------------------------------
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "TeamMember")]
        public async Task<ActionResult<TaskReturnDto>> UpdateTaskStatus(int id, UpdateTaskStatusDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
                return Unauthorized(new ApiResponse(401));

            var result = await _taskService.UpdateTaskStatusAsync(id, dto.Status, currentUser.Id);

            if (result is null)
                return NotFound(new ApiResponse(404, "Task not found or not assigned to you"));

            return Ok(_mapper.Map<TaskReturnDto>(result));
        }

        // -------------------------------------------------------
        // GET /api/tasks/my-tasks
        // TeamMember only - get tasks assigned to me
        // -------------------------------------------------------
        [HttpGet("my-tasks")]
        [Authorize(Roles = "TeamMember")]
        public async Task<ActionResult<IReadOnlyList<TaskReturnDto>>> GetMyTasks()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
                return Unauthorized(new ApiResponse(401));

            var tasks = await _taskService.GetMyTasksAsync(currentUser.Id);
            return Ok(_mapper.Map<IReadOnlyList<TaskReturnDto>>(tasks));
        }

        // GET /api/tasks/my-team-tasks
        // TeamLeader only - gets tasks for HIS team only
        [HttpGet("my-team-tasks")]
        [Authorize(Roles = "TeamLeader")]
        public async Task<ActionResult<IReadOnlyList<TaskReturnDto>>> GetTeamTasks()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(currentUserId!);

            if (currentUser?.Team is null)
                return BadRequest(new ApiResponse(400, "TeamLeader has no team assigned"));

            var tasks = await _taskService.GetTeamTasksAsync(currentUser.Team.Value, currentUserId!);
            return Ok(_mapper.Map<IReadOnlyList<TaskReturnDto>>(tasks));
        }


        // -------------------------------------------------------
        // GET /api/tasks/upcoming-deadlines
        // Any authenticated user - tasks due within next 24 hours
        // AND status != Completed
        // -------------------------------------------------------
        [HttpGet("upcoming-deadlines")]
        public async Task<ActionResult<IReadOnlyList<TaskReturnDto>>> GetUpcomingDeadlines()
        {
            var tasks = await _taskService.GetUpcomingDeadlinesAsync();
            return Ok(_mapper.Map<IReadOnlyList<TaskReturnDto>>(tasks));
        }

        // -------------------------------------------------------
        // GET /api/tasks/{id}
        // Any authenticated user - get task by id
        // -------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskReturnDto>> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task is null)
                return NotFound(new ApiResponse(404, "Task not found"));

            return Ok(_mapper.Map<TaskReturnDto>(task));
        }

    }
}
