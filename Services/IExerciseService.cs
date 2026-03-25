using System;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;

namespace WorkoutTrackerAPI.Services;

public interface IExerciseService
{
    Task<List<ExerciseResponse>> GetAllExercisesAsync(string userId);
    Task<ExerciseResponse> GetExerciseByIdAsync(Guid id, string userId);
    Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest request, string userId);
    Task UpdateExerciseAsync(Guid id, UpdateExerciseRequest request, string userId);
    Task DeleteExerciseAsync(Guid id, string userId);
}
