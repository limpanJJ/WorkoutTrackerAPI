using WorkoutTrackerAPI.Dtos;

namespace WorkoutTrackerAPI.Services
{
    public interface IAuthService
    {
        Task<UserResponse> RegisterAsync(RegisterRequest request);
    }
}
