using System;
using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Dtos;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public class ExerciseService(AppDbContext context) : IExerciseService
    {
        public async Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest request)
        {
            var newExercise = new Exercise
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                MuscleGroupId = request.MuscleGroupId,
                CreatedAt = DateTime.UtcNow
            };

            context.Exercises.Add(newExercise);
            await context.SaveChangesAsync();

            await context.Entry(newExercise).Reference(e => e.Category).LoadAsync();
            if (newExercise.MuscleGroupId.HasValue)
            {
                await context.Entry(newExercise).Reference(e => e.MuscleGroup).LoadAsync();
            }

            return MapToResponse(newExercise);
        }

        public async Task<bool> DeleteExerciseAsync(Guid id)
        {
            var exerciseToDelete = await context.Exercises.FindAsync(id);
            if (exerciseToDelete is null)
            {
                return false;
            }

            context.Exercises.Remove(exerciseToDelete);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ExerciseResponse>> GetAllExercisesAsync()
            => await context.Exercises
                .AsNoTracking()
                .Include(e => e.Category)
                .Include(e => e.MuscleGroup)
                .OrderBy(e => e.Name)
                .Select(e => new ExerciseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category.Name,
                    MuscleGroupId = e.MuscleGroupId,
                    MuscleGroupName = e.MuscleGroup != null ? e.MuscleGroup.Name : null,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

        public async Task<ExerciseResponse?> GetExerciseByIdAsync(Guid id)
            => await context.Exercises
                .AsNoTracking()
                .Include(e => e.Category)
                .Include(e => e.MuscleGroup)
                .Where(e => e.Id == id)
                .Select(e => new ExerciseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category.Name,
                    MuscleGroupId = e.MuscleGroupId,
                    MuscleGroupName = e.MuscleGroup != null ? e.MuscleGroup.Name : null,
                    CreatedAt = e.CreatedAt
                })
                .FirstOrDefaultAsync();

        public async Task<bool> UpdateExerciseAsync(Guid id, UpdateExerciseRequest request)
        {
            var existingExercise = await context.Exercises.FindAsync(id);
            if (existingExercise is null)
            {
                return false;
            }

            existingExercise.Name = request.Name;
            existingExercise.CategoryId = request.CategoryId;
            existingExercise.MuscleGroupId = request.MuscleGroupId;

            await context.SaveChangesAsync();
            return true;
        }

        private static ExerciseResponse MapToResponse(Exercise exercise)
            => new()
            {
                Id = exercise.Id,
                Name = exercise.Name,
                CategoryId = exercise.CategoryId,
                CategoryName = exercise.Category.Name,
                MuscleGroupId = exercise.MuscleGroupId,
                MuscleGroupName = exercise.MuscleGroup?.Name,
                CreatedAt = exercise.CreatedAt
            };
    }
}
