using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos.Common;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

/// <summary>Endpoints for managing exercises.</summary>
/// <remarks>Users can only manage their own exercises.</remarks>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ExerciseController(IExerciseService service) : ControllerBase
{
    /// <summary>Returns a paginated list of exercises.</summary>
    /// <response code="200">Exercises returned successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ExerciseResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ExerciseResponse>>> GetExercises(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await service.GetAllExercisesAsync(GetUserId(), page, pageSize));

    /// <summary>Returns a specific exercise.</summary>
    /// <response code="200">Exercise returned successfully.</response>
    /// <response code="404">Exercise not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExerciseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseResponse>> GetExerciseById(Guid id)
        => Ok(await service.GetExerciseByIdAsync(id, GetUserId()));

    /// <summary>Creates a new exercise.</summary>
    /// <response code="201">Exercise created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="409">An exercise with the same name already exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ExerciseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ExerciseResponse>> CreateExercise(CreateExerciseRequest request)
    {
        var createdExercise = await service.CreateExerciseAsync(request, GetUserId());
        return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id }, createdExercise);
    }

    /// <summary>Updates an existing exercise.</summary>
    /// <response code="204">Exercise updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Exercise not found.</response>
    /// <response code="409">An exercise with the same name already exists.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateExercise(Guid id, UpdateExerciseRequest request)
    {
        await service.UpdateExerciseAsync(id, request, GetUserId());
        return NoContent();
    }

    /// <summary>Deletes an exercise.</summary>
    /// <response code="204">Exercise deleted successfully.</response>
    /// <response code="404">Exercise not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteExercise(Guid id)
    {
        await service.DeleteExerciseAsync(id, GetUserId());
        return NoContent();
    }

    private string GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? throw new UnauthorizedAccessException();
}
