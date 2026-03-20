using System.ComponentModel.DataAnnotations;

namespace WorkoutTrackerAPI.Dtos.Auth.Requests
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, MinLength(6)]
        public required string Password { get; set; }
    }
}
