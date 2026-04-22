using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutTrackerAPI.Dtos.Common;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

/// <summary>Endpoints for retrieving workout statistics.</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class StatsController(IWorkoutStatsService service) : ControllerBase
{
    /// <summary>Returns all-time workout stats.</summary>
    /// <response code="200">Stats returned successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(WorkoutStatsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkoutStatsResponse>> GetStats()
        => Ok(await service.GetAllTimeStatsAsync(GetUserId()));

    /// <summary>Returns yearly workout stats.</summary>
    /// <param name="year">The year to retrieve stats for, e.g. 2025.</param>
    /// <response code="200">Stats returned successfully.</response>
    [HttpGet("{year:int}")]
    [ProducesResponseType(typeof(WorkoutStatsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkoutStatsResponse>> GetStatsByYear(int year)
        => Ok(await service.GetYearlyStatsAsync(GetUserId(), year));

    /// <summary>Returns monthly workout stats.</summary>
    /// <param name="year">The year, e.g. 2025.</param>
    /// <param name="month">The month as a number, e.g. 3 for March.</param>
    /// <response code="200">Stats returned successfully.</response>
    [HttpGet("{year:int}/{month:int}")]
    [ProducesResponseType(typeof(WorkoutStatsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkoutStatsResponse>> GetStatsByMonth(int year, int month)
        => Ok(await service.GetMonthlyStatsAsync(GetUserId(), year, month));

    private string GetUserId()
    => User.FindFirstValue(ClaimTypes.NameIdentifier)
       ?? throw new UnauthorizedAccessException();
}

