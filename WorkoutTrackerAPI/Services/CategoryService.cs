using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Dtos.Exercises.Categories.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Categories.Responses;
using WorkoutTrackerAPI.Exceptions;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
    public class CategoryService(AppDbContext context) : ICategoryService
    {
        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await context.ExerciseCategories.FindAsync(id)
                ?? throw new NotFoundException($"Category with ID {id} was not found.");
            return MapToResponse(category);
        }


        public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
            => await context.ExerciseCategories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

        public async Task UpdateCategoryAsync(int id, UpdateCategoryRequest request)
        {
            var existingCategory = await context.ExerciseCategories.FindAsync(id)
                ?? throw new NotFoundException($"Category with ID {id} was not found.");

            await EnsureNameIsUniqueAsync(request.Name, id);

            existingCategory.Name = request.Name;
            await context.SaveChangesAsync();
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
        {
            await EnsureNameIsUniqueAsync(request.Name);

            var newCategory = new ExerciseCategory
            {
                Name = request.Name
            };

            context.ExerciseCategories.Add(newCategory);
            await context.SaveChangesAsync();

            return MapToResponse(newCategory);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await context.ExerciseCategories.FindAsync(id)
                ?? throw new NotFoundException($"Category with ID {id} was not found.");

            context.ExerciseCategories.Remove(category);
            await context.SaveChangesAsync();
        }

        private async Task EnsureNameIsUniqueAsync(string name, int? excludeId = null)
        {
            var exists = await context.ExerciseCategories
                .AnyAsync(c => c.Name == name && (excludeId == null || c.Id != excludeId));

            if (exists)
                throw new ConflictException($"Category '{name}' already exists.");
        }

        private static CategoryResponse MapToResponse(ExerciseCategory category)
            => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
    }
}
