using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Constants;

namespace WorkoutTrackerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExerciseCategoriesController() : ControllerBase
{
	[HttpGet]
	public IActionResult GetCategories()
	{
		throw new NotImplementedException();
	}
	[HttpGet("{id:int}")]
	public IActionResult GetCategoryById(int id)
	{
		throw new NotImplementedException();
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpPost]
	public IActionResult CreateCategory()
	{
		throw new NotImplementedException();
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpPut("{id:int}")]
	public IActionResult UpdateCategory(int id)
	{
		throw new NotImplementedException();
	}

	[Authorize(Roles = Roles.Admin)]
	[HttpDelete("{id:int}")]
	public IActionResult DeleteCategory(int id)
	{
		throw new NotImplementedException();
	}
}
