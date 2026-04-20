using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Repositories
{
    public interface IWorkoutSessionRepository
    {
        // Workout Sessions
        Task<List<WorkoutSession>> GetAllWorkoutsAsync(string userId, WorkoutSessionQueryParameters p);
        Task<int> CountWorkoutsAsync(string userId, WorkoutSessionQueryParameters p);
        Task<WorkoutSession?> GetWorkoutByIdAsync(Guid id, string userId, bool tracked = false);
        Task<WorkoutSession> CreateWorkoutAsync(WorkoutSession workoutSession);
        Task DeleteWorkoutAsync(WorkoutSession workoutSession);

        // Workout Exercises
        Task<WorkoutExercise?> GetWorkoutExerciseByIdAsync(Guid workoutSessionId, Guid exerciseId, string userId, bool tracked = false);
        Task<WorkoutExercise> AddWorkoutExerciseAsync(WorkoutExercise workoutExercise);
        Task DeleteWorkoutExerciseAsync(WorkoutExercise workoutExercise);

        // Exercise Sets
        Task<WorkoutExerciseSet?> GetExerciseSetByIdAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, string userId, bool tracked = false);
        Task<WorkoutExerciseSet> AddExerciseSetAsync(WorkoutExerciseSet exerciseSet);
        Task DeleteExerciseSetAsync(WorkoutExerciseSet exerciseSet);

        Task SaveChangesAsync();

    }
}
