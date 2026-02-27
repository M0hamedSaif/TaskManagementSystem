namespace TaskManager.APIs.Dto.DashboardDtos
{
    public class DashboardSummaryDto
    {
        public int TotalTasks { get; set; }
        public int CompletedCount { get; set; }
        public int InProgressCount { get; set; }
        public int PendingCount { get; set; }
        public int OverdueCount { get; set; }

        // Key = Team name, Value = task count
        public Dictionary<string, int> TasksPerTeam { get; set; } = new();
    }
}
