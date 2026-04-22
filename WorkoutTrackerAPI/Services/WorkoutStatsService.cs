using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Dtos.Common;

namespace WorkoutTrackerAPI.Services
{
    public class WorkoutStatsService(AppDbContext context) : IWorkoutStatsService
    {
        public async Task<WorkoutStatsResponse> GetAllTimeStatsAsync(string userId)
    => await GetStatsAsync(userId, DateTime.MinValue, DateTime.MaxValue);

        public async Task<WorkoutStatsResponse> GetYearlyStatsAsync(string userId, int year)
            => await GetStatsAsync(userId, new DateTime(year, 1, 1), new DateTime(year, 12, 31, 23, 59, 59));

        public async Task<WorkoutStatsResponse> GetMonthlyStatsAsync(string userId, int year, int month)
            => await GetStatsAsync(userId, new DateTime(year, month, 1), new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59));

        private async Task<WorkoutStatsResponse> GetStatsAsync(string userId, DateTime start, DateTime end)
        {
            var sets = context.WorkoutExerciseSets
                .Where(wes => wes.WorkoutExercise.WorkoutSession.UserId == userId
                    && wes.WorkoutExercise.WorkoutSession.StartedAt >= start
                    && wes.WorkoutExercise.WorkoutSession.StartedAt <= end);

            return new WorkoutStatsResponse
            {
                TotalSessions = await context.WorkoutSessions.CountAsync(ws => ws.UserId == userId && ws.StartedAt >= start && ws.StartedAt <= end),
                TotalExercises = await context.WorkoutExercises.CountAsync(we => we.WorkoutSession.UserId == userId && we.WorkoutSession.StartedAt >= start && we.WorkoutSession.StartedAt <= end),
                TotalSets = await sets.CountAsync(),
                TotalReps = await sets.SumAsync(s => s.Reps ?? 0),
                TotalWeight = await sets.SumAsync(s => s.Weight ?? 0)
            };
        }

    }
}
