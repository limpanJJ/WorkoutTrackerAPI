using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Services;
using WorkoutTrackerAPI.Dtos;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController(IExerciseService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ExerciseResponse>>> GetExercises() 
        => Ok(await service.GetAllExercisesAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ExerciseResponse>> GetExerciseById(int id)
    {
        var exercise = await service.GetExerciseByIdAsync(id);
        return exercise is null
            ? NotFound("Exercise with the given ID was not found.")
            : Ok(exercise);
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseResponse>> CreateExercise(CreateExerciseRequest request)
    {
        var createdExercise = await service.CreateExerciseAsync(request);
        return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id }, createdExercise);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateExercise(int id, UpdateExerciseRequest request)
    {
        var updated = await service.UpdateExerciseAsync(id, request);
        return updated ? NoContent() : NotFound("Exercise with the given Id was not found.");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteExercise(int id)
    {
        var deleted = await service.DeleteExerciseAsync(id);
        return deleted ? NoContent() : NotFound("Exervise with the given Id was not found.");
    }
}
