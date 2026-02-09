using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public class ExerciseService : IExerciseService
    {

        static List<Exercise> exercises = new List<Exercise>
        {
            new Exercise { Id = 1, Name = "Push-Up", BodyType = "Chest", Category = "Strength", CreatedAt = DateTime.UtcNow },
            new Exercise { Id = 2, Name = "Squat", BodyType = "Quads", Category = "Strength", CreatedAt = DateTime.UtcNow },
            new Exercise { Id = 3, Name = "Plank", BodyType = "Core", Category = "Strength", CreatedAt = DateTime.UtcNow }
        };

        public Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteExerciseAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Exercise?> GetExerciseByIdAsync(int id)
        {
            var result = exercises.FirstOrDefault(e => e.Id == id);
            return await Task.FromResult(result);
        }

        public async Task<List<Exercise>> GetAllExercisesAsync()
            => await Task.FromResult(exercises);

        public Task<bool> UpdateExerciseAsync(int id, Exercise exercise)
        {
            throw new NotImplementedException();
        }

    }
}
