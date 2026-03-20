using System;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;

namespace WorkoutTrackerAPI.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseResponse>> GetAllExercisesAsync(string userId);
        Task<ExerciseResponse?> GetExerciseByIdAsync(Guid id, string userId);
        Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest exercise, string userId);
        Task<bool> UpdateExerciseAsync(Guid id, UpdateExerciseRequest exercise, string userId);
        Task<bool> DeleteExerciseAsync(Guid id, string userId);
    }
}
