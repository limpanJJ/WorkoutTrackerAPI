using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Constants;
using WorkoutTrackerAPI.Dtos.Exercises.Categories.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Categories.Responses;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExerciseCategoriesController(ICategoryService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetCategories()
        => Ok(await service.GetAllCategoriesAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponse>> GetCategoryById(int id)
        => Ok(await service.GetCategoryByIdAsync(id));

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> CreateCategory(CreateCategoryRequest request)
    {
        var createdCategory = await service.CreateCategoryAsync(request);
        return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateCategory(int id, UpdateCategoryRequest request)
    {
        await service.UpdateCategoryAsync(id, request);
        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        await service.DeleteCategoryAsync(id);
        return NoContent();
    }
}
