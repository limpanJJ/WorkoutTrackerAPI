using WorkoutTrackerAPI.Dtos;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseResponse>> GetAllExercisesAsync();
        Task<ExerciseResponse?> GetExerciseByIdAsync(int id);
        Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest exercise);
        Task<bool> UpdateExerciseAsync(int id, UpdateExerciseRequest exercise);
        Task<bool> DeleteExerciseAsync(int id);
    }
}
