using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Specifications.Tasks
{
    public class OverdueTaskSpecification : BaseSpecification<AppTask>
    {
        public OverdueTaskSpecification()
            : base(t => t.Deadline < DateTime.UtcNow
                     && t.Status != AppTaskStatus.Completed)
        {
        }
    }
}
