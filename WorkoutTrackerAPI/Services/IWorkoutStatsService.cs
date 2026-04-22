using WorkoutTrackerAPI.Dtos.Common;

namespace WorkoutTrackerAPI.Services
{
    public interface IWorkoutStatsService
    {
        Task<WorkoutStatsResponse> GetAllTimeStatsAsync(string userId);
        Task<WorkoutStatsResponse> GetYearlyStatsAsync(string userId, int year);
        Task<WorkoutStatsResponse> GetMonthlyStatsAsync(string userId, int year, int month);
    }
}
