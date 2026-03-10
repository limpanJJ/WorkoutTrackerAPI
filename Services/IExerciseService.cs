using System;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.Responses;

namespace WorkoutTrackerAPI.Services
{
    public interface IExerciseService
    {
        Task<List<ExerciseResponse>> GetAllExercisesAsync();
        Task<ExerciseResponse?> GetExerciseByIdAsync(Guid id);
        Task<ExerciseResponse> CreateExerciseAsync(CreateExerciseRequest exercise);
        Task<bool> UpdateExerciseAsync(Guid id, UpdateExerciseRequest exercise);
        Task<bool> DeleteExerciseAsync(Guid id);
    }
}
