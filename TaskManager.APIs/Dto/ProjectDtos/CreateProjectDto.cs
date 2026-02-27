using System.ComponentModel.DataAnnotations;

namespace TaskManager.APIs.Dto.ProjectDtos
{
    public class CreateProjectDto
    {
        [Required]
        [MaxLength(200)]
        public string ClientName { get; set; } = null!;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "DesignCost must be positive")]
        public decimal DesignCost { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "ProductionCost must be positive")]
        public decimal ProductionCost { get; set; }
    }
}
