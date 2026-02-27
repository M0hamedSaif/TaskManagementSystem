using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Specifications.Tasks
{
    public class TaskSpecification : BaseSpecification<AppTask>
    {
        public TaskSpecification(Team team)
           : base(t => t.Team == team)
        {
            AddIncludes();
            AddOrderByDesc(t => t.CreatedAt);
        }

        // Get all tasks assigned to a specific user
        public TaskSpecification(string userId)
            : base(t => t.AssignedToUserId == userId)
        {
            AddIncludes();
            AddOrderByDesc(t => t.CreatedAt);
        }

        // Get single task by id (with navigation properties loaded)
        public TaskSpecification(int taskId)
            : base(t => t.Id == taskId)
        {
            AddIncludes();
        }

        // Get upcoming deadlines:
        // deadline within next 24 hours AND status not Completed
        public TaskSpecification(DateTime cutoff)
            : base(t => t.Deadline <= cutoff && t.Status != AppTaskStatus.Completed)
        {
            AddIncludes();
            AddOrderBy(t => t.Deadline);
        }

        // Get all tasks (used by dashboard)
        public TaskSpecification()
            : base()
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(t => t.AssignedTo);
            Includes.Add(t => t.CreatedBy);
        }
    }
}
