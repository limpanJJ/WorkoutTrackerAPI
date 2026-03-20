using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Services;
using WorkoutTrackerAPI.Routes;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;

namespace WorkoutTrackerAPI.Controllers;

[Route(ApiRoutes.Exercises.Base)]
[ApiController]
[Authorize]
public class ExerciseController(IExerciseService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ExerciseResponse>>> GetExercises()
        => Ok(await service.GetAllExercisesAsync(GetUserId()));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseResponse>> GetExerciseById(Guid id)
    {
        var exercise = await service.GetExerciseByIdAsync(id, GetUserId());
        return exercise is null
            ? NotFound("Exercise with the given ID was not found.")
            : Ok(exercise);
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseResponse>> CreateExercise(CreateExerciseRequest request)
    {
        var createdExercise = await service.CreateExerciseAsync(request, GetUserId());
        return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id }, createdExercise);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateExercise(Guid id, UpdateExerciseRequest request)
    {
        var updated = await service.UpdateExerciseAsync(id, request, GetUserId());
        return updated ? NoContent() : NotFound("Exercise with the given Id was not found.");
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteExercise(Guid id)
    {
        var deleted = await service.DeleteExerciseAsync(id, GetUserId());
        return deleted ? NoContent() : NotFound("Exercise with the given Id was not found.");
    }

    private string GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? throw new UnauthorizedAccessException();
}
