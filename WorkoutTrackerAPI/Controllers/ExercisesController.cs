using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Dtos.Common;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExercisesController(IExerciseService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ExerciseResponse>>> GetExercises(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await service.GetAllExercisesAsync(GetUserId(), page, pageSize));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseResponse>> GetExerciseById(Guid id)
        => Ok(await service.GetExerciseByIdAsync(id, GetUserId()));

    [HttpPost]
    public async Task<ActionResult<ExerciseResponse>> CreateExercise(CreateExerciseRequest request)
    {
        var createdExercise = await service.CreateExerciseAsync(request, GetUserId());
        return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id }, createdExercise);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateExercise(Guid id, UpdateExerciseRequest request)
    {
        await service.UpdateExerciseAsync(id, request, GetUserId());
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteExercise(Guid id)
    {
        await service.DeleteExerciseAsync(id, GetUserId());
        return NoContent();
    }

    private string GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
           ?? throw new UnauthorizedAccessException();
}
