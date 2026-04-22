using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Dtos.Common;

namespace WorkoutTrackerAPI.Services
{
    public class WorkoutStatsService(AppDbContext context) : IWorkoutStatsService
    {
        public async Task<WorkoutStatsResponse> GetAllTimeStatsAsync(string userId)
        {
            var sets = context.WorkoutExerciseSets
                .Where(wes => wes.WorkoutExercise.WorkoutSession.UserId == userId);

            return new WorkoutStatsResponse
            {
                TotalSessions = await context.WorkoutSessions.CountAsync(ws => ws.UserId == userId),
                TotalExercises = await context.WorkoutExercises.CountAsync(we => we.WorkoutSession.UserId == userId),
                TotalSets = await sets.CountAsync(),
                TotalReps = await sets.SumAsync(s => s.Reps ?? 0),
                TotalWeight = await sets.SumAsync(s => s.Weight ?? 0)
            };
        }

        public async Task<WorkoutStatsResponse> GetYearlyStatsAsync(string userId, int year)
        {
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31, 23, 59, 59);

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

        public async Task<WorkoutStatsResponse> GetMonthlyStatsAsync(string userId, int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

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
