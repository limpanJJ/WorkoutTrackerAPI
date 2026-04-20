using System.ComponentModel.DataAnnotations;

namespace WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;

public class UpdateWorkoutSessionRequest
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}