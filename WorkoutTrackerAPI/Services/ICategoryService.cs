using System;
using WorkoutTrackerAPI.Dtos.Exercises.Categories.Responses;
using WorkoutTrackerAPI.Dtos.Exercises.Categories.Requests;

namespace WorkoutTrackerAPI.Services;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllCategoriesAsync();
    Task<CategoryResponse> GetCategoryByIdAsync(int id);
    Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request);
    Task UpdateCategoryAsync(int id, UpdateCategoryRequest request);
    Task DeleteCategoryAsync(int id);
}
