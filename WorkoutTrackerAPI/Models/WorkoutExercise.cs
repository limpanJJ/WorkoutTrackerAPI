namespace WorkoutTrackerAPI.Models;

/// <summary>
/// Represents a specific exercise performed during a <see cref="WorkoutSession"/>.
/// Uses <see cref="Exercise"/> as a template — the session-specific data (sets, order, notes) lives here.
/// </summary>
public class WorkoutExercise
{
    public Guid Id { get; set; }

    public Guid WorkoutSessionId { get; set; }
    public WorkoutSession WorkoutSession { get; set; } = null!;

    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public int Order { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<WorkoutExerciseSet> WorkoutExerciseSets { get; set; } = new List<WorkoutExerciseSet>();
}
