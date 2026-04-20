using WorkoutTrackerAPI.Dtos.Common;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;

namespace WorkoutTrackerAPI.Services;

public interface IExerciseService
{
    Task<PagedResponse<ExerciseResponse>> GetAllExercisesAsync(string userId, int page, int pageSize);
    Task<ExerciseResponse> GetExerciseByIdAsync(Guid id, string userId);
    Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest request, string userId);
    Task UpdateExerciseAsync(Guid id, UpdateExerciseRequest request, string userId);
    Task DeleteExerciseAsync(Guid id, string userId);
}
