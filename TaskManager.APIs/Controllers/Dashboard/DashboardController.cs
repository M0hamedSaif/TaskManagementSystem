using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.APIs.Dto.DashboardDtos;
using TaskManager.Core.Services.Contract;

namespace TaskManager.APIs.Controllers.Dashboard
{
    [Authorize(Roles = "Admin,TeamLeader")]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;

        public DashboardController(IDashboardService dashboardService, IMapper mapper)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }

        // GET /api/dashboard/summary
        // Admin + TeamLeader only - full task statistics
        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
        {
            var result = await _dashboardService.GetSummaryAsync();
            return Ok(_mapper.Map<DashboardSummaryDto>(result));
        }
    }
}

