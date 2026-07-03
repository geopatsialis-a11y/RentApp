using API.DTOs.Dashboard;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class DashboardController(IDashboardService dashboardService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<DashboardDto>> Get()
    {
        return Ok(await dashboardService.GetAsync());
    }
}
