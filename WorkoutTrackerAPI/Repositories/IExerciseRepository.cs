using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Repositories
{
    public interface IExerciseRepository
    {
        Task<List<Exercise>> GetAllExercisesAsync(string userId);
        Task<Exercise?> GetExerciseByIdAsync(Guid id, string userId, bool tracked = false);
        Task<Exercise> CreateExerciseAsync(Exercise exercise);
        Task DeleteExerciseAsync(Exercise exercise);
        Task<bool> ExistsAsync(string name, string userId);
        Task SaveChangesAsync();
    }
}
