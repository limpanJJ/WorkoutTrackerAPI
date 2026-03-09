namespace WorkoutTrackerAPI.Models;

/// <summary>
/// Represents a workout session for a <see cref="User"/>,
/// containing one or more <see cref="SessionExercise"/> entries each with their <see cref="ExerciseSet"/>s.
/// </summary>
public class WorkoutSession
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? Notes { get; set; }

    public ICollection<SessionExercise> SessionExercises { get; set; } = new List<SessionExercise>();
}
