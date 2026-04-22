using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkoutTrackerAPI.Dtos.Common;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Responses;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Responses;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Responses;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;
/// <summary>Endpoints for managing workout sessions, exercises, and sets.</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class WorkoutSessionController(IWorkoutSessionService service) : ControllerBase
{
    // === Workout Sessions ===

    /// <summary>Returns a paginated list of workout sessions with optional filtering and sorting.</summary>
    /// <response code="200">Sessions returned successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<WorkoutSessionSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<WorkoutSessionSummaryResponse>>> GetWorkoutSessions(
    [FromQuery] WorkoutSessionQueryParameters parameters)
    => Ok(await service.GetAllWorkoutSessionsAsync(GetUserId(), parameters));


    /// <summary>Returns a specific workout session.</summary>
    /// <response code="200">Session returned successfully.</response>
    /// <response code="404">Session not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WorkoutSessionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutSessionResponse>> GetWorkoutSessionById(Guid id)
        => Ok(await service.GetWorkoutSessionByIdAsync(id, GetUserId()));

    /// <summary>Creates a new workout session.</summary>
    /// <response code="201">Session created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    [HttpPost]
    [ProducesResponseType(typeof(WorkoutSessionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WorkoutSessionResponse>> CreateWorkoutSession(CreateWorkoutSessionRequest request)
    {
        var created = await service.CreateWorkoutSessionAsync(request, GetUserId());
        return CreatedAtAction(nameof(GetWorkoutSessionById), new { id = created.Id }, created);
    }

    /// <summary>Updates an existing workout session.</summary>
    /// <response code="204">Session updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Session not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateWorkoutSession(Guid id, UpdateWorkoutSessionRequest request)
    {
        await service.UpdateWorkoutAsync(id, request, GetUserId());
        return NoContent();
    }

    /// <summary>Deletes a workout session.</summary>
    /// <response code="204">Session deleted successfully.</response>
    /// <response code="404">Session not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteWorkoutSession(Guid id)
    {
        await service.DeleteWorkoutSessionAsync(id, GetUserId());
        return NoContent();
    }

    // === Workout Exercises ===

    /// <summary>Adds an exercise to a workout session.</summary>
    /// <response code="201">Exercise added successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Session not found.</response>
    [HttpPost("{workoutSessionId:guid}/Exercises")]
    [ProducesResponseType(typeof(WorkoutExerciseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddExerciseToWorkout(Guid workoutSessionId, CreateWorkoutExerciseRequest request)
    {
        var created = await service.AddExerciseToWorkoutAsync(workoutSessionId, request, GetUserId());
        return CreatedAtAction(nameof(GetWorkoutSessionById), new { id = workoutSessionId }, created);
    }

    /// <summary>Updates an exercise in a workout session.</summary>
    /// <response code="204">Exercise updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Session or exercise not found.</response>
    [HttpPut("{workoutSessionId:guid}/Exercises/{exerciseId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateWorkoutExercise(Guid workoutSessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request)
    {
        await service.UpdateWorkoutExerciseAsync(workoutSessionId, exerciseId, request, GetUserId());
        return NoContent();
    }

    /// <summary>Removes an exercise from a workout session.</summary>
    /// <response code="204">Exercise removed successfully.</response>
    /// <response code="404">Session or exercise not found.</response>
    [HttpDelete("{workoutSessionId:guid}/Exercises/{exerciseId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteWorkoutExercise(Guid workoutSessionId, Guid exerciseId)
    {
        await service.DeleteWorkoutExerciseAsync(workoutSessionId, exerciseId, GetUserId());
        return NoContent();
    }

    // === Exercise Sets ===

    /// <summary>Adds a set to an exercise in a workout session.</summary>
    /// <response code="201">Set added successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Session or exercise not found.</response>
    [HttpPost("{workoutSessionId:guid}/Exercises/{exerciseId:guid}/Sets")]
    [ProducesResponseType(typeof(ExerciseSetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddExerciseSet(Guid workoutSessionId, Guid exerciseId, CreateExerciseSetRequest request)
    {
        var created = await service.AddExerciseSetAsync(workoutSessionId, exerciseId, request, GetUserId());
        return CreatedAtAction(nameof(GetWorkoutSessionById), new { id = workoutSessionId }, created);
    }

    /// <summary>Updates a set in an exercise.</summary>
    /// <response code="204">Set updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Session, exercise or set not found.</response>
    [HttpPut("{workoutSessionId:guid}/Exercises/{exerciseId:guid}/Sets/{setId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateExerciseSet(Guid workoutSessionId, Guid exerciseId, Guid setId, UpdateExerciseSetRequest request)
    {
        await service.UpdateExerciseSetAsync(workoutSessionId, exerciseId, setId, request, GetUserId());
        return NoContent();
    }

    /// <summary>Deletes a set from an exercise.</summary>
    /// <response code="204">Set deleted successfully.</response>
    /// <response code="404">Session, exercise or set not found.</response>
    [HttpDelete("{workoutSessionId:guid}/Exercises/{exerciseId:guid}/Sets/{setId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteExerciseSet(Guid workoutSessionId, Guid exerciseId, Guid setId)
    {
        await service.DeleteExerciseSetAsync(workoutSessionId, exerciseId, setId, GetUserId());
        return NoContent();
    }

    private string GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? throw new UnauthorizedAccessException();
}
