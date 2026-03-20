using System;
using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public class ExerciseService(AppDbContext context) : IExerciseService
    {
        public async Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest request, string userId)
        {
            var newExercise = new Exercise
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                MuscleGroupId = request.MuscleGroupId,
                UserId = userId,
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

        public async Task<bool> DeleteExerciseAsync(Guid id, string userId)
        {
            var exercise = await context.Exercises.FindAsync(id);
            if (exercise is null || !IsOwnedBy(exercise, userId))
                return false;

            context.Exercises.Remove(exercise);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ExerciseResponse>> GetAllExercisesAsync(string userId)
            => await context.Exercises
                .AsNoTracking()
                .Include(e => e.Category)
                .Include(e => e.MuscleGroup)
                .Where(e => e.UserId == null || e.UserId == userId)
                .OrderBy(e => e.Name)
                .Select(e => new ExerciseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category.Name,
                    MuscleGroupId = e.MuscleGroupId,
                    MuscleGroupName = e.MuscleGroup != null ? e.MuscleGroup.Name : null,
                    IsDefault = e.UserId == null,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

        public async Task<ExerciseResponse?> GetExerciseByIdAsync(Guid id, string userId)
            => await context.Exercises
                .AsNoTracking()
                .Include(e => e.Category)
                .Include(e => e.MuscleGroup)
                .Where(e => e.Id == id && (e.UserId == null || e.UserId == userId))
                .Select(e => new ExerciseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    CategoryId = e.CategoryId,
                    CategoryName = e.Category.Name,
                    MuscleGroupId = e.MuscleGroupId,
                    MuscleGroupName = e.MuscleGroup != null ? e.MuscleGroup.Name : null,
                    IsDefault = e.UserId == null,
                    CreatedAt = e.CreatedAt
                })
                .FirstOrDefaultAsync();

        public async Task<bool> UpdateExerciseAsync(Guid id, UpdateExerciseRequest request, string userId)
        {
            var existingExercise = await context.Exercises.FindAsync(id);
            if (existingExercise is null || !IsOwnedBy(existingExercise, userId))
                return false;

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
                IsDefault = exercise.UserId is null,
                CreatedAt = exercise.CreatedAt
            };

        /// <summary>
        /// Only the owner can modify/delete their custom exercises.
        /// Default exercises (UserId is null) are not editable through this service.
        /// </summary>
        private static bool IsOwnedBy(Exercise exercise, string userId)
            => exercise.UserId is not null && exercise.UserId == userId;
    }
}
