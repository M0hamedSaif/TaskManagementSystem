using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Services.Contract;
using TaskManager.Core.Specifications.Tasks;

namespace TaskManager.Service.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TaskService> _logger;

        public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<AppTask> CreateTaskAsync(AppTask task)
        {
            await _unitOfWork.Repository<AppTask>().AddAsync(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }

        public async Task<AppTask?> UpdateTaskAsync(int taskId, AppTask updatedTask, string requestingUserId)
        {
            var spec = new TaskSpecification(taskId);
            var task = await _unitOfWork.Repository<AppTask>().GetWithSpecAsync(spec);

            if (task is null) return null;

            // Why check CreatedByUserId? -> Only the TeamLeader who created
            //                               this task is allowed to update it
            //                               prevents one leader editing another's tasks
            if (task.CreatedByUserId != requestingUserId) return null;

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.AssignedToUserId = updatedTask.AssignedToUserId;
            task.Team = updatedTask.Team;
            task.Deadline = updatedTask.Deadline;
            // Why not update Status here? -> Status is updated separately
            //                                by TeamMember via UpdateTaskStatusAsync
            //                                separating concerns prevents
            //                                leaders from bypassing member workflow

            _unitOfWork.Repository<AppTask>().Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }

        public async Task<AppTask?> UpdateTaskStatusAsync(int taskId, AppTaskStatus newStatus, string requestingUserId)
        {
            var spec = new TaskSpecification(taskId);
            var task = await _unitOfWork.Repository<AppTask>().GetWithSpecAsync(spec);

            if (task is null) return null;

            if (task.AssignedToUserId != requestingUserId) return null;

            task.Status = newStatus;

            _unitOfWork.Repository<AppTask>().Update(task);
            await _unitOfWork.CompleteAsync();
            return task;
        }

        public async Task<IReadOnlyList<AppTask>> GetTeamTasksAsync(Team team, string requestingUserId)
        {
            var spec = new TaskSpecification(team);
            return await _unitOfWork.Repository<AppTask>().GetAllWithSpecAsync(spec);
            // Why no requestingUserId filter here? -> The controller already
            //                                         verified the caller is a TeamLeader
            //                                         and passes their team value
            //                                         service just fetches by team
        }

        public async Task<IReadOnlyList<AppTask>> GetMyTasksAsync(string userId)
        {
            var spec = new TaskSpecification(userId);
            return await _unitOfWork.Repository<AppTask>().GetAllWithSpecAsync(spec);
        }

        public async Task<AppTask?> GetTaskByIdAsync(int taskId)
        {
            var spec = new TaskSpecification(taskId);
            return await _unitOfWork.Repository<AppTask>().GetWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<AppTask>> GetUpcomingDeadlinesAsync()
        {
            var cutoff = DateTime.UtcNow.AddHours(24);
            var spec = new TaskSpecification(cutoff);
            var tasks = await _unitOfWork.Repository<AppTask>().GetAllWithSpecAsync(spec);

            // Simulate sending email reminder for each upcoming task
            // WHY logging? -> Real email requires SMTP setup out of scope
            //                 logging simulates the notification behavior
            foreach (var task in tasks)
            {
                _logger.LogInformation(
                    "[EMAIL REMINDER] Task '{Title}' assigned to {User} is due at {Deadline}. Please complete it on time.",
                    task.Title,
                    task.AssignedTo?.DisplayName ?? task.AssignedToUserId,
                    task.Deadline.ToString("yyyy-MM-dd HH:mm") + " UTC"
                );
            }

            return tasks;
        }

        public async Task<int> GetTaskCountByStatusAsync(AppTaskStatus status)
        {
            var spec = new TaskByStatusSpecification(status);
            return await _unitOfWork.Repository<AppTask>().GetCountAsync(spec);
        }

        public async Task<int> GetOverdueTasksCountAsync()
        {
            var spec = new OverdueTaskSpecification();
            return await _unitOfWork.Repository<AppTask>().GetCountAsync(spec);
        }

        public async Task<IReadOnlyList<AppTask>> GetAllTasksAsync()
        {
            var spec = new TaskSpecification();
            return await _unitOfWork.Repository<AppTask>().GetAllWithSpecAsync(spec);
        }
    }
}
