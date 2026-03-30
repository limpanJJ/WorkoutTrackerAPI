using System.ComponentModel.DataAnnotations;

namespace WorkoutTrackerAPI.Dtos.Exercises.Requests
{
    public class UpdateExerciseRequest
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public int? MuscleGroupId { get; set; }
    }
}
