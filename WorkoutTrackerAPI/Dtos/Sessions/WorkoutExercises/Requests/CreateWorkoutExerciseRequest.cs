using System.ComponentModel.DataAnnotations;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;

namespace WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;

public class CreateWorkoutExerciseRequest
{
    [Required]
    public Guid ExerciseId { get; set; }

    [Range(1, int.MaxValue)]
    public int Order { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public List<CreateExerciseSetRequest> Sets { get; set; } = [];
}
