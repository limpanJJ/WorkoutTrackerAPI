namespace WorkoutTrackerAPI.Models;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string? Username { get; set; }
    public DateOnly CreatedAt { get; set; }
}
