using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Identity;

namespace TaskManager.Core.Entities.Tasks
{
    public class AppTask : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AssignedToUserId { get; set; } = null!;
        public ApplicationUser AssignedTo { get; set; } = null!;
        public string CreatedByUserId { get; set; } = null!;
        public ApplicationUser CreatedBy { get; set; } = null!;
        public Team Team { get; set; }
        public AppTaskStatus Status { get; set; } = AppTaskStatus.Pending;
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
