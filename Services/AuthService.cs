using Microsoft.AspNetCore.Identity;
using WorkoutTrackerAPI.Dtos;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                // returnera errors, eller kasta exception med dem
            }
            return new UserResponse
            {
                Id = user.Id,
                Username = user.UserName,
                CreatedAtUtc = user.CreatedAtUtc
            };
        }

    }

}
