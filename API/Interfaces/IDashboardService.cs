using API.DTOs.Dashboard;

namespace API.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetAsync();
}
