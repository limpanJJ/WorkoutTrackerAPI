using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Models;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorukoutExerciseController(IExerciseService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Exercise>>> GetExercises() 
        => Ok(await service.GetAllExercisesAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Exercise>> GetExerciseById(int id)
    {
        var exercise = await service.GetExerciseByIdAsync(id);
        return exercise is null
            ? NotFound("Exercise with the given ID was not found.")
            : Ok(exercise);

    }
}
