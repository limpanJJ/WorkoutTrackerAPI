using Microsoft.AspNetCore.Identity;

namespace WorkoutTrackerAPI.Models;

public class User : IdentityUser
{
    public DateTimeOffset CreatedAtUtc { get; init; }
}
