using System.ComponentModel.DataAnnotations;

namespace WorkoutTrackerAPI.Dtos.Exercises.Categories.Requests
{
    public class UpdateCategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
