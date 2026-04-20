using WorkoutTrackerAPI.Dtos.Common;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;
using WorkoutTrackerAPI.Exceptions;
using WorkoutTrackerAPI.Models;
using WorkoutTrackerAPI.Repositories;

namespace WorkoutTrackerAPI.Services
{
    public class ExerciseService(IExerciseRepository repository) : IExerciseService
    {
        public async Task<PagedResponse<ExerciseResponse>> GetAllExercisesAsync(string userId, int page, int pageSize)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var totalCount = await repository.CountExercisesAsync(userId);
            var exercises = await repository.GetAllExercisesAsync(userId, page, pageSize);

            return new PagedResponse<ExerciseResponse>
            {
                Items = exercises.Select(MapToResponse).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ExerciseResponse> GetExerciseByIdAsync(Guid id, string userId)
        {
            var exercise = await repository.GetExerciseByIdAsync(id, userId)
                ?? throw new NotFoundException($"Exercise with ID {id} was not found.");

            return MapToResponse(exercise);
        }

        public async Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest request, string userId)
        {
            if (await repository.ExistsAsync(request.Name, userId))
                throw new ConflictException($"An exercise with the name '{request.Name}' already exists.");

            var exercise = new Exercise
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                MuscleGroupId = request.MuscleGroupId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await repository.CreateExerciseAsync(exercise);
            return MapToResponse(created);
        }

        public async Task UpdateExerciseAsync(Guid id, UpdateExerciseRequest request, string userId)
        {
            var exercise = await repository.GetExerciseByIdAsync(id, userId, tracked: true)
                ?? throw new NotFoundException($"Exercise with ID {id} was not found.");

            if (exercise.UserId != userId)
                throw new UnauthorizedException("You do not have permission to update this exercise.");

            exercise.Name = request.Name;
            exercise.CategoryId = request.CategoryId;
            exercise.MuscleGroupId = request.MuscleGroupId;

            await repository.SaveChangesAsync();

        }

        public async Task DeleteExerciseAsync(Guid id, string userId)
        {

            var exercise = await repository.GetExerciseByIdAsync(id, userId, tracked: true)
                ?? throw new NotFoundException($"Exercise with ID {id} was not found.");

            if (exercise.UserId != userId)
                throw new UnauthorizedException("You do not have permission to delete this exercise.");

            await repository.DeleteExerciseAsync(exercise);

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
    }
}
