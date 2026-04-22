using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StatsController(IWorkoutStatsService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetStats()
        => Ok(await service.GetAllTimeStatsAsync(GetUserId()));

    [HttpGet("{year:int}")]
    public async Task<ActionResult> GetStatsByYear(int year)
        => Ok(await service.GetYearlyStatsAsync(GetUserId(), year));

    [HttpGet("{year:int}/{month:int}")]
    public async Task<ActionResult> GetStatsByMonth(int year, int month)
        => Ok(await service.GetMonthlyStatsAsync(GetUserId(), year, month));

    private string GetUserId()
    => User.FindFirstValue(ClaimTypes.NameIdentifier)
       ?? throw new UnauthorizedAccessException();
}

