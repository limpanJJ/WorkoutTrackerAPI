using Microsoft.EntityFrameworkCore;    
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Dtos;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public class ExerciseService(AppDbContext context) : IExerciseService
    {

        public async Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest exercise)
        {
            var newExercise = new Exercise
            {
                Name = exercise.Name,
                BodyType = exercise.BodyType,
                Category = exercise.Category,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };
            context.Add(newExercise);
            await context.SaveChangesAsync();
            return new ExerciseResponse
            {
                Id = newExercise.Id,
                BodyType = newExercise.BodyType,
                Category = newExercise.Category
            };
        }

        public async Task<bool> DeleteExerciseAsync(int id)
        {
            var exerciseToDelete = await context.Exercises.FindAsync(id);
            if (exerciseToDelete is null) 
                return false;

            context.Exercises.Remove(exerciseToDelete);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ExerciseResponse>> GetAllExercisesAsync()
            => await context.Exercises.Select(e => new ExerciseResponse
            {
                Id = e.Id,
                Name = e.Name,
                BodyType = e.BodyType,
                Category = e.Category
            }).ToListAsync();

        public async Task<ExerciseResponse?> GetExerciseByIdAsync(int id)
        {
            var result = await context.Exercises
                .Where(e => e.Id == id)
                .Select(e => new ExerciseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    BodyType = e.BodyType,
                    Category = e.Category
                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<bool> UpdateExerciseAsync(int id, UpdateExerciseRequest exercise)
        {
            var existingCharacter = await context.Exercises.FindAsync(id);
            if (existingCharacter is null) 
                return false;

            existingCharacter.Name = exercise.Name;
            existingCharacter.BodyType = exercise.BodyType;
            existingCharacter.Category = exercise.Category;

            await context.SaveChangesAsync();
            return true;
        }

    }
}
