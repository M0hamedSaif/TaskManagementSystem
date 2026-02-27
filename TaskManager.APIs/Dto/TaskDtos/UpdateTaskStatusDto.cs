using System.ComponentModel.DataAnnotations;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.APIs.Dto.TaskDtos
{
    public class UpdateTaskStatusDto
    {
        [Required]
        public AppTaskStatus Status { get; set; }
        
    }
}
