namespace WorkoutTrackerAPI.Dtos
{
    public class UserResponse
    {
        public string? Id { get; init; }
        public required string Username { get; init; }
        public DateTimeOffset CreatedAtUtc { get; init; }
    }
}
