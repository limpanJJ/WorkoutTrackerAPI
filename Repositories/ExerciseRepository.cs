using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Repositories
{
	public class ExerciseRepository(AppDbContext context) : IExerciseRepository
	{
		public async Task<List<Exercise>> GetAllExercisesAsync(string userId)
			=> await context.Exercises
				.AsNoTracking()
				.Include(e => e.Category)
				.Include(e => e.MuscleGroup)
				.Where(e => e.UserId == null || e.UserId == userId)
				.OrderBy(e => e.Name)
				.ToListAsync();

		public async Task<Exercise?> GetExerciseByIdAsync(Guid id, string userId, bool tracked = false)
		{
			var query = context.Exercises
				.Include(e => e.Category)
				.Include(e => e.MuscleGroup)
				.Where(e => e.Id == id && (e.UserId == null || e.UserId == userId));

			if (!tracked)
				query = query.AsNoTracking();

			return await query.FirstOrDefaultAsync();
		}

		public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
		{
			context.Exercises.Add(exercise);
			await context.SaveChangesAsync();

			await context.Entry(exercise).Reference(e => e.Category).LoadAsync();
			if (exercise.MuscleGroupId.HasValue)
			{
				await context.Entry(exercise).Reference(e => e.MuscleGroup).LoadAsync();
			}

			return exercise;
		}

		public async Task SaveChangesAsync() => await context.SaveChangesAsync();

		public async Task<bool> DeleteExerciseAsync(Guid id, string userId)
		{
			var exercise = await context.Exercises
				.Where(e => e.Id == id && e.UserId == userId)
				.FirstOrDefaultAsync();

			if (exercise is null)
				return false;

			context.Exercises.Remove(exercise);
			await context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> ExistsAsync(string name, string userId)
			=> await context.Exercises
				.AnyAsync(e => e.Name == name && (e.UserId == null || e.UserId == userId));

	}
}
