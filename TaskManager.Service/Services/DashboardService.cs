using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core;
using TaskManager.Core.Entities.Dashboard;
using TaskManager.Core.Entities.Tasks;
using TaskManager.Core.Services.Contract;
using TaskManager.Core.Specifications.Tasks;

namespace TaskManager.Service.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardSummaryResult> GetSummaryAsync()
        {
            // Get all tasks with navigation properties loaded
            var allTasksSpec = new TaskSpecification();
            var allTasks = await _unitOfWork.Repository<AppTask>()
                .GetAllWithSpecAsync(allTasksSpec);

            var completedCount = allTasks.Count(t => t.Status == AppTaskStatus.Completed);
            var inProgressCount = allTasks.Count(t => t.Status == AppTaskStatus.InProgress);
            var pendingCount = allTasks.Count(t => t.Status == AppTaskStatus.Pending);

            // Overdue: deadline passed AND not completed
            var overdueCount = allTasks.Count(t =>
                t.Deadline < DateTime.UtcNow &&
                t.Status != AppTaskStatus.Completed);

            // Group by team, convert enum to string for clean JSON output
            var tasksPerTeam = allTasks
                .GroupBy(t => t.Team.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            return new DashboardSummaryResult
            {
                TotalTasks = allTasks.Count,
                CompletedCount = completedCount,
                InProgressCount = inProgressCount,
                PendingCount = pendingCount,
                OverdueCount = overdueCount,
                TasksPerTeam = tasksPerTeam
            };
        }
    }
}

