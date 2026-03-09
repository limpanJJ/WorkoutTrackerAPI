using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Routes;

namespace WorkoutTrackerAPI.Controllers
{
	[Route(ApiRoutes.ExerciseCategories.Base)]
	[ApiController]
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

		[HttpPost]
		public IActionResult CreateCategory()
		{
			throw new NotImplementedException();
		}
		[HttpPut("{id:int}")]
		public IActionResult UpdateCategory(int id)
		{
			throw new NotImplementedException();
		}

		[HttpDelete("{id:int}")]
		public IActionResult DeleteCategory(int id)
		{
			throw new NotImplementedException();
		}

	}
}
