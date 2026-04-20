using System.ComponentModel.DataAnnotations;

namespace WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Requests
{
    public class UpdateMuscleGroupRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}