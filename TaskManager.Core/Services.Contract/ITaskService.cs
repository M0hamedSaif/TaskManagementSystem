using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Tasks;


namespace TaskManager.Core.Services.Contract
{
    public interface ITaskService
    {
        // TeamLeader operations
        Task<AppTask> CreateTaskAsync(AppTask task);
        Task<AppTask?> UpdateTaskAsync(int taskId, AppTask updatedTask, string requestingUserId);
        Task<IReadOnlyList<AppTask>> GetTeamTasksAsync(Team team, string requestingUserId);

        // TeamMember operations
        Task<IReadOnlyList<AppTask>> GetMyTasksAsync(string userId);
        Task<AppTask?> UpdateTaskStatusAsync(int taskId, AppTaskStatus newStatus, string requestingUserId);

        // Shared operations
        Task<AppTask?> GetTaskByIdAsync(int taskId);

        // Reminder operation (used in Step 5)
        Task<IReadOnlyList<AppTask>> GetUpcomingDeadlinesAsync();

        // Dashboard operation (used in Step 6)
        Task<int> GetTaskCountByStatusAsync(AppTaskStatus status);
        Task<int> GetOverdueTasksCountAsync();
        Task<IReadOnlyList<AppTask>> GetAllTasksAsync();
    }
}
