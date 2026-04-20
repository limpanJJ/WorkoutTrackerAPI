using WorkoutTrackerAPI.Dtos.Common;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Responses;

namespace WorkoutTrackerAPI.Services
{
    public interface IWorkoutSessionService
    {
        Task<PagedResponse<WorkoutSessionSummaryResponse>> GetAllWorkoutSessionsAsync(string userId, WorkoutSessionQueryParameters parameters);
        Task<WorkoutSessionResponse> GetWorkoutSessionByIdAsync(Guid id, string userId);
        Task<WorkoutSessionResponse> CreateWorkoutSessionAsync(CreateWorkoutSessionRequest request, string userId);
        Task DeleteWorkoutSessionAsync(Guid id, string userId);
        Task UpdateWorkoutAsync(Guid id, UpdateWorkoutSessionRequest request, string userId);
        Task AddExerciseToWorkoutAsync(Guid workoutSessionId, CreateWorkoutExerciseRequest request, string userId);
        Task UpdateWorkoutExerciseAsync(Guid workoutSessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request, string userId);
        Task DeleteWorkoutExerciseAsync(Guid workoutSessionId, Guid exerciseId, string userId);
        Task AddExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, CreateExerciseSetRequest request, string userId);
        Task UpdateExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, UpdateExerciseSetRequest request, string userId);
        Task DeleteExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, string userId);
    }
}
