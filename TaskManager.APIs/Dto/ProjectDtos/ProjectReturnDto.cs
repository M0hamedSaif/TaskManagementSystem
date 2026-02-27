namespace TaskManager.APIs.Dto.ProjectDtos
{
    public class ProjectReturnDto
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = null!;
        public decimal DesignCost { get; set; }
        public decimal ProductionCost { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
