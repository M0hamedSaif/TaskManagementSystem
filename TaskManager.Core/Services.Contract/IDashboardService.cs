using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Dashboard;

namespace TaskManager.Core.Services.Contract
{
    public interface IDashboardService
    {
        // Returns the full dashboard summary for Admin/TeamLeader
        Task<DashboardSummaryResult> GetSummaryAsync();
    }
}
