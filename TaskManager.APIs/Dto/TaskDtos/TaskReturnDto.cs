namespace TaskManager.APIs.Dto.TaskDtos
{
    public class TaskReturnDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AssignedToUserId { get; set; } = null!;
        public string AssignedToName { get; set; } = null!;
        public string CreatedByUserId { get; set; } = null!;
        public string CreatedByName { get; set; } = null!;
        public string Team { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
