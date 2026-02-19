using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}