using System.ComponentModel.DataAnnotations;

namespace WorkoutTrackerAPI.Dtos.Exercises.Categories.Requests
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
