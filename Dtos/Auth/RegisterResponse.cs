namespace WorkoutTrackerAPI.Dtos.Auth
{
    public class RegisterResponse
    {
        public string? Id { get; init; }
        public required string Username { get; init; }
        public DateTimeOffset CreatedAtUtc { get; init; }
    }
}   