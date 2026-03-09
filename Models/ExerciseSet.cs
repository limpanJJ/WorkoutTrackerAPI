namespace WorkoutTrackerAPI.Models;

/// <summary>
/// Represents a single set performed within a <see cref="SessionExercise"/>.
/// Note: <see cref="Exercise"/> is a reusable template — the actual workout data lives here.
/// </summary>
public class ExerciseSet
{
    public Guid Id { get; set; }

    public Guid SessionExerciseId { get; set; }
    public SessionExercise SessionExercise { get; set; } = null!;

    public int SetNumber { get; set; }
    public int? Reps { get; set; }
    public decimal? Weight { get; set; }
    public int? DurationSeconds { get; set; }
    public decimal? DistanceMeters { get; set; }
    public DateTime CreatedAt { get; set; }
}
