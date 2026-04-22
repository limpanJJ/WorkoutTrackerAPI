using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Constants;
using WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Responses;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;
/// <summary>Endpoints for managing muscle groups.</summary>
/// <remarks>
/// Muscle groups are used to classify exercises, e.g. Chest, Back, Legs.
/// Admin-only for create/update/delete operations.
/// </remarks>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class MuscleGroupsController(IMuscleGroupService service) : ControllerBase
{
    /// <summary>Returns all muscle groups.</summary>
    /// <response code="200">Muscle groups returned successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<MuscleGroupResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MuscleGroupResponse>>> GetMuscleGroups()
        => Ok(await service.GetAllMuscleGroupsAsync());

    /// <summary>Returns a specific muscle group.</summary>
    /// <response code="200">Muscle group returned successfully.</response>
    /// <response code="404">Muscle group not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MuscleGroupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MuscleGroupResponse>> GetMuscleGroupById(int id)
        => Ok(await service.GetMuscleGroupByIdAsync(id));

    /// <summary>Creates a new muscle group. Admin only.</summary>
    /// <response code="201">Muscle group created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="409">A muscle group with the same name already exists.</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(MuscleGroupResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MuscleGroupResponse>> CreateMuscleGroup(CreateMuscleGroupRequest request)
    {
        var createdMuscleGroup = await service.CreateMuscleGroupAsync(request);
        return CreatedAtAction(nameof(GetMuscleGroupById), new { id = createdMuscleGroup.Id }, createdMuscleGroup);
    }

    /// <summary>Updates an existing muscle group. Admin only.</summary>
    /// <response code="204">Muscle group updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Muscle group not found.</response>
    /// <response code="409">A muscle group with the same name already exists.</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateMuscleGroup(int id, UpdateMuscleGroupRequest request)
    {
        await service.UpdateMuscleGroupAsync(id, request);
        return NoContent();
    }

    /// <summary>Deletes a muscle group. Admin only.</summary>
    /// <response code="204">Muscle group deleted successfully.</response>
    /// <response code="404">Muscle group not found.</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteMuscleGroup(int id)
    {
        await service.DeleteMuscleGroupAsync(id);
        return NoContent();
    }
}