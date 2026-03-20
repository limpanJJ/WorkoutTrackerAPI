using Microsoft.AspNetCore.Identity;
using WorkoutTrackerAPI.Constants;
using WorkoutTrackerAPI.Dtos.Auth.Requests;
using WorkoutTrackerAPI.Dtos.Auth.Responses;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthService> logger,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
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
                _logger.LogWarning("Registration failed for {Email}: {Errors}", request.Email, errors);
                throw new InvalidOperationException(errors);
            }

            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

            await _userManager.AddToRoleAsync(user, Roles.User);

            _logger.LogInformation("User registered successfully: {UserId}", user.Id);
            return new RegisterResponse
            {
                Id = user.Id,
                Username = user.UserName,
                CreatedAtUtc = user.CreatedAtUtc
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                _logger.LogWarning("Login attempt with non-existent email: {Email}", request.Email);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Login attempt with invalid password for user: {UserId}", user.Id);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            _logger.LogInformation("User logged in successfully: {UserId}", user.Id);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            return new LoginResponse
            {
                Id = user.Id,
                Username = user.UserName!,
                Token = token
            };
        }
    }
}
