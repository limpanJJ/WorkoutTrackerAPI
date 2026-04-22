using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Constants;
using WorkoutTrackerAPI.Dtos.Exercises.Categories.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Categories.Responses;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;
/// <summary>Endpoints for managing exercise categories.</summary>
/// <remarks>
/// Categories are used to classify exercises, e.g. Strength, Cardio, Mobility.
/// Admin-only for create/update/delete operations.
/// </remarks>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ExerciseCategoriesController(ICategoryService service) : ControllerBase
{
    /// <summary>Returns all categories.</summary>
    /// <response code="200">Categories returned successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryResponse>>> GetCategories()
        => Ok(await service.GetAllCategoriesAsync());


    /// <summary>Returns a category.</summary>
    /// <response code="200">Category returned successfully.</response>
    /// <response code="404">Category not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById(int id)
        => Ok(await service.GetCategoryByIdAsync(id));

    /// <summary>Creates a new category. Admin only.</summary>
    /// <response code="201">Category created successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="409">A category with the same name already exists.</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryResponse>> CreateCategory(CreateCategoryRequest request)
    {
        var createdCategory = await service.CreateCategoryAsync(request);
        return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
    }

    /// <summary>Updates an existing category. Admin only.</summary>
    /// <response code="204">Category updated successfully.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="404">Category not found.</response>
    /// <response code="409">A category with the same name already exists.</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateCategory(int id, UpdateCategoryRequest request)
    {
        await service.UpdateCategoryAsync(id, request);
        return NoContent();
    }

    /// <summary>Deletes a category. Admin only.</summary>
    /// <response code="204">Category deleted successfully.</response>
    /// <response code="404">Category not found.</response>
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        await service.DeleteCategoryAsync(id);
        return NoContent();
    }
}
