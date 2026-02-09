using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public interface IExerciseService
    {
        Task<List<Exercise>> GetAllExercisesAsync();
        Task<Exercise?> GetExerciseByIdAsync(int id);
        Task<Exercise> CreateExerciseAsync(Exercise exercise);
        Task<bool> UpdateExerciseAsync(int id, Exercise exercise);
        Task<bool> DeleteExerciseAsync(int id);
    }
}
