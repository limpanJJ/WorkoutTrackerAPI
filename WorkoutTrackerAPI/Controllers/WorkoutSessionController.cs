using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Services;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Responses;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WorkoutSessionController(IWorkoutSessionService service) : ControllerBase
{
    // === Workout Sessions ===

    [HttpGet]
    public async Task<ActionResult<List<WorkoutSessionSummaryResponse>>> GetWorkoutSessions()
        => Ok(await service.GetAllWorkoutSessionsAsync(GetUserId()));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkoutSessionResponse>> GetWorkoutSessionById(Guid id)
        => Ok(await service.GetWorkoutSessionByIdAsync(id, GetUserId()));

    [HttpPost]
    public async Task<ActionResult<WorkoutSessionResponse>> CreateWorkoutSession(CreateWorkoutSessionRequest request)
    {
        var created = await service.CreateWorkoutSessionAsync(request, GetUserId());
        return CreatedAtAction(nameof(GetWorkoutSessionById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateWorkoutSession(Guid id, UpdateWorkoutSessionRequest request)
    {
        await service.UpdateWorkoutAsync(id, request, GetUserId());
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteWorkoutSession(Guid id)
    {
        await service.DeleteWorkoutSessionAsync(id, GetUserId());
        return NoContent();
    }

    // === Workout Exercises ===

    [HttpPost("{workoutSessionId:guid}/Exercises")]
    public async Task<ActionResult> AddExerciseToWorkout(Guid workoutSessionId, CreateWorkoutExerciseRequest request)
    {
        await service.AddExerciseToWorkoutAsync(workoutSessionId, request, GetUserId());
        return NoContent();
    }

    [HttpPut("{workoutSessionId:guid}/Exercises/{exerciseId:guid}")]
    public async Task<ActionResult> UpdateWorkoutExercise(Guid workoutSessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request)
    {
        await service.UpdateWorkoutExerciseAsync(workoutSessionId, exerciseId, request, GetUserId());
        return NoContent();
    }

    [HttpDelete("{workoutSessionId:guid}/Exercises/{exerciseId:guid}")]
    public async Task<ActionResult> DeleteWorkoutExercise(Guid workoutSessionId, Guid exerciseId)
    {
        await service.DeleteWorkoutExerciseAsync(workoutSessionId, exerciseId, GetUserId());
        return NoContent();
    }

    // === Exercise Sets ===

    [HttpPost("{workoutSessionId:guid}/Exercises/{exerciseId:guid}/Sets")]
    public async Task<ActionResult> AddExerciseSet(Guid workoutSessionId, Guid exerciseId, CreateExerciseSetRequest request)
    {
        await service.AddExerciseSetAsync(workoutSessionId, exerciseId, request, GetUserId());
        return NoContent();
    }

    [HttpPut("{workoutSessionId:guid}/Exercises/{exerciseId:guid}/Sets/{setId:guid}")]
    public async Task<ActionResult> UpdateExerciseSet(Guid workoutSessionId, Guid exerciseId, Guid setId, UpdateExerciseSetRequest request)
    {
        await service.UpdateExerciseSetAsync(workoutSessionId, exerciseId, setId, request, GetUserId());
        return NoContent();
    }

    [HttpDelete("{workoutSessionId:guid}/Exercises/{exerciseId:guid}/Sets/{setId:guid}")]
    public async Task<ActionResult> DeleteExerciseSet(Guid workoutSessionId, Guid exerciseId, Guid setId)
    {
        await service.DeleteExerciseSetAsync(workoutSessionId, exerciseId, setId, GetUserId());
        return NoContent();
    }

    private string GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? throw new UnauthorizedAccessException();
}
