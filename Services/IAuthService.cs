using WorkoutTrackerAPI.Dtos;

namespace WorkoutTrackerAPI.Services
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
