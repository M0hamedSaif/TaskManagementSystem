using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Specifications.Tasks
{
    public class TaskByStatusSpecification : BaseSpecification<AppTask>
    {
        public TaskByStatusSpecification(AppTaskStatus status)
            : base(t => t.Status == status)
        {
            
        }
    }
}
