using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Repositories
{
	public interface IExerciseRepository
	{
		Task<List<Exercise>> GetAllExercisesAsync(string userId);
		Task<Exercise?> GetExerciseByIdAsync(Guid id, string userId, bool tracked = false);
		Task<Exercise> CreateExerciseAsync(Exercise exercise);
		Task<bool> DeleteExerciseAsync(Guid id, string userId);
		Task<bool> ExistsAsync(string name, string userId);
		Task SaveChangesAsync();
	}
}
