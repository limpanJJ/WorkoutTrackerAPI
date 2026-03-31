using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Constants;
using WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Responses;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MuscleGroupsController(IMuscleGroupService service) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<List<MuscleGroupResponse>>> GetMuscleGroups()
		=> Ok(await service.GetAllMuscleGroupsAsync());

	[HttpGet("{id:int}")]
	public async Task<ActionResult<MuscleGroupResponse>> GetMuscleGroupById(int id)
		=> Ok(await service.GetMuscleGroupByIdAsync(id));

	[Authorize(Roles = Roles.Admin)]
	[HttpPost]
	public async Task<ActionResult<MuscleGroupResponse>> CreateMuscleGroup(CreateMuscleGroupRequest request)
	{
		var createdMuscleGroup = await service.CreateMuscleGroupAsync(request);
		return CreatedAtAction(nameof(GetMuscleGroupById), new { id = createdMuscleGroup.Id }, createdMuscleGroup);
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpPut("{id:int}")]
	public async Task<ActionResult> UpdateMuscleGroup(int id, UpdateMuscleGroupRequest request)
	{
		await service.UpdateMuscleGroupAsync(id, request);
		return NoContent();
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpDelete("{id:int}")]
	public async Task<ActionResult> DeleteMuscleGroup(int id)
	{
		await service.DeleteMuscleGroupAsync(id);
		return NoContent();
	}
}