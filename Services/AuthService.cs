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
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }
            return new UserResponse
            {
                Id = user.Id,
                Username = user.UserName,
                CreatedAtUtc = user.CreatedAtUtc
            };
        }
        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            // Verify user's Email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Verify user's Password
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                throw new InvalidOperationException("Invalid email or password.");
            }

            return new UserResponse
            {
                Id = user.Id,
                Username = user.UserName!,
                CreatedAtUtc = user.CreatedAtUtc
            };
        }

    }

}
