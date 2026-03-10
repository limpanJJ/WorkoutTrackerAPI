using WorkoutTrackerAPI.Dtos.Auth.Requests;
using WorkoutTrackerAPI.Dtos.Auth.Responses;

namespace WorkoutTrackerAPI.Services
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
