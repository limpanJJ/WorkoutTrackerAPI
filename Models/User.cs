using Microsoft.AspNetCore.Identity;

namespace WorkoutTrackerAPI.Models;

/// <summary>
/// Represents a registered user. Extends <see cref="IdentityUser"/> which provides
/// Id, Email, UserName, PasswordHash etc.
/// </summary>
public class User : IdentityUser
{
    public DateTimeOffset CreatedAtUtc { get; init; }
}
