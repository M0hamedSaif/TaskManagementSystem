using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Entities.Projects
{
    public class Project : BaseEntity
    {
        public string ClientName { get; set; } = null!;
        public decimal DesignCost { get; set; }
        public decimal ProductionCost { get; set; }

        // WHY calculated here not in DB?
        // -> Always stays in sync when either cost changes
        //    service recalculates before every save
        public decimal TotalPrice { get; set; }

        // Default is Draft - project starts as a quote
        public ProjectStatus Status { get; set; } = ProjectStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
