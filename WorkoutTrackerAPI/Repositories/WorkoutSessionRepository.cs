using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Repositories
{
    public class WorkoutSessionRepository(AppDbContext context) : IWorkoutSessionRepository
    {
        // Workout Sessions
        public async Task<List<WorkoutSession>> GetAllWorkoutsAsync(string userId)
            => await context.WorkoutSessions
                .AsNoTracking()
                .Include(ws => ws.WorkoutExercises)
                    .ThenInclude(we => we.WorkoutExerciseSets)
                .Where(ws => ws.UserId == userId)
                .OrderBy(ws => ws.StartedAt)
                .ToListAsync();


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

        public Task DeleteWorkoutAsync(WorkoutSession workoutSession)
            => throw new NotImplementedException();

        // Workout Exercises
        public Task<WorkoutExercise?> GetWorkoutExerciseByIdAsync(Guid workoutSessionId, Guid exerciseId, string userId, bool tracked = false)
            => throw new NotImplementedException();

        public Task<WorkoutExercise> AddWorkoutExerciseAsync(WorkoutExercise workoutExercise)
            => throw new NotImplementedException();

        public Task DeleteWorkoutExerciseAsync(WorkoutExercise workoutExercise)
            => throw new NotImplementedException();

        // Exercise Sets
        public Task<WorkoutExerciseSet?> GetExerciseSetByIdAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, string userId, bool tracked = false)
            => throw new NotImplementedException();

        public Task<WorkoutExerciseSet> AddExerciseSetAsync(WorkoutExerciseSet exerciseSet)
            => throw new NotImplementedException();

        public Task DeleteExerciseSetAsync(WorkoutExerciseSet exerciseSet)
            => throw new NotImplementedException();

        public Task SaveChangesAsync()
            => throw new NotImplementedException();
    }
}
