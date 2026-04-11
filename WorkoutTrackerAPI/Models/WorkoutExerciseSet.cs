namespace WorkoutTrackerAPI.Models;

/// <summary>
/// Represents a single set performed within a <see cref="WorkoutExercise"/>.
/// Note: <see cref="Exercise"/> is a reusable template — the actual workout data lives here.
/// </summary>
public class WorkoutExerciseSet
{
    public Guid Id { get; set; }

    public Guid WorkoutExerciseId { get; set; }
    public WorkoutExercise WorkoutExercise { get; set; } = null!;

    public int SetNumber { get; set; }
    public int? Reps { get; set; }
    public decimal? Weight { get; set; }
    public int? DurationSeconds { get; set; }
    public decimal? DistanceMeters { get; set; }
}
