using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Repositories
{
    public class WorkoutSessionRepository(AppDbContext context) : IWorkoutSessionRepository
    {
        // Workout Sessions
        public async Task<List<WorkoutSession>> GetAllWorkoutsAsync(string userId, WorkoutSessionQueryParameters p)
            => await ApplySorting(BuildQuery(userId, p), p)
                .Include(ws => ws.WorkoutExercises)
                    .ThenInclude(we => we.WorkoutExerciseSets)
                .Skip((p.Page - 1) * p.PageSize)
                .Take(p.PageSize)
                .ToListAsync();

        public async Task<int> CountWorkoutsAsync(string userId, WorkoutSessionQueryParameters p)
            => await BuildQuery(userId, p).CountAsync();


        public async Task<WorkoutSession?> GetWorkoutByIdAsync(Guid id, string userId, bool tracked = false)
        {
            var query = context.WorkoutSessions
                .Include(ws => ws.WorkoutExercises)
                    .ThenInclude(we => we.WorkoutExerciseSets)
                .Where(ws => ws.Id == id && (ws.UserId == userId));

            if (!tracked)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<WorkoutSession> CreateWorkoutAsync(WorkoutSession workoutSession)
        {
            context.WorkoutSessions.Add(workoutSession);
            await context.SaveChangesAsync();

            return workoutSession;
        }

        public async Task DeleteWorkoutAsync(WorkoutSession workoutSession)
        {
            context.WorkoutSessions.Remove(workoutSession);
            await context.SaveChangesAsync();
        }

        // Workout Exercises
        public Task<WorkoutExercise?> GetWorkoutExerciseByIdAsync(Guid workoutSessionId, Guid exerciseId, string userId, bool tracked = false)
        {
            var query = context.WorkoutExercises
                .Include(we => we.WorkoutExerciseSets)
                .Where(we => we.WorkoutSessionId == workoutSessionId
                    && we.ExerciseId == exerciseId
                    && we.WorkoutSession.UserId == userId);
            if (!tracked)
                query = query.AsNoTracking();
            return query.FirstOrDefaultAsync();
        }

        public async Task<WorkoutExercise> AddWorkoutExerciseAsync(WorkoutExercise workoutExercise)
        {
            context.WorkoutExercises.Add(workoutExercise);

            await context.SaveChangesAsync();

            return workoutExercise;
        }

        public async Task DeleteWorkoutExerciseAsync(WorkoutExercise workoutExercise)
        {
            context.WorkoutExercises.Remove(workoutExercise);
            await context.SaveChangesAsync();
        }

        // Exercise Sets
        public async Task<WorkoutExerciseSet?> GetExerciseSetByIdAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, string userId, bool tracked = false)
        {
            var query = context.WorkoutExerciseSets
                .Where(wes => wes.Id == setId
                    && wes.WorkoutExerciseId == exerciseId
                    && wes.WorkoutExercise.WorkoutSessionId == workoutSessionId
                    && wes.WorkoutExercise.WorkoutSession.UserId == userId);

            if (!tracked)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<WorkoutExerciseSet> AddExerciseSetAsync(WorkoutExerciseSet exerciseSet)
        {
            context.WorkoutExerciseSets.Add(exerciseSet);
            await context.SaveChangesAsync();

            return exerciseSet;
        }


        public Task DeleteExerciseSetAsync(WorkoutExerciseSet exerciseSet)
        {
            context.WorkoutExerciseSets.Remove(exerciseSet);
            return context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await context.SaveChangesAsync();

        private IQueryable<WorkoutSession> BuildQuery(string userId, WorkoutSessionQueryParameters p)
        {
            var query = context.WorkoutSessions
                .AsNoTracking()
                .Where(ws => ws.UserId == userId);

            if (!string.IsNullOrWhiteSpace(p.Name))
                query = query.Where(ws => ws.Name.Contains(p.Name));

            if (p.StartDate.HasValue)
                query = query.Where(ws => ws.StartedAt >= p.StartDate.Value);

            if (p.EndDate.HasValue)
                query = query.Where(ws => ws.StartedAt <= p.EndDate.Value);

            return query;
        }

        private static IQueryable<WorkoutSession> ApplySorting(IQueryable<WorkoutSession> query, WorkoutSessionQueryParameters p)
        {
            bool asc = p.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase);

            return p.SortBy.ToLowerInvariant() switch
            {
                "name" => asc ? query.OrderBy(ws => ws.Name) : query.OrderByDescending(ws => ws.Name),
                _ => asc ? query.OrderBy(ws => ws.StartedAt) : query.OrderByDescending(ws => ws.StartedAt),
            };
        }

    }
}
