using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.APIs.Dto.TaskDtos
{
    public class CreateTaskDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;

        [Required]
        public string AssignedToUserId { get; set; } = null!;

        [Required]
        public Team Team { get; set; }

        [Required]
        public DateTime Deadline { get; set; }
    }
}
