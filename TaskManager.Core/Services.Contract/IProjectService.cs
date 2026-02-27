using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Projects;

namespace TaskManager.Core.Services.Contract
{
    public interface IProjectService
    {
        // Admin/TeamLeader - create new project quote
        Task<Project> CreateAsync(Project project);

        // Admin/TeamLeader - update costs, recalculates TotalPrice
        Task<Project?> UpdateCostsAsync(int id, decimal designCost, decimal productionCost);

        // Admin/TeamLeader - mark project as sent to client
        Task<Project?> ChangeStatusToSentAsync(int id);
    }
}
