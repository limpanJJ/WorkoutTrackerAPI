using System.ComponentModel.DataAnnotations;

namespace WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;

public class UpdateWorkoutExerciseRequest
{
    [Range(1, int.MaxValue)]
    public int Order { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
