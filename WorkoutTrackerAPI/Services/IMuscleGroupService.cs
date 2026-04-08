using WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Responses;

namespace WorkoutTrackerAPI.Services;

public interface IMuscleGroupService
{
    Task<List<MuscleGroupResponse>> GetAllMuscleGroupsAsync();
    Task<MuscleGroupResponse> GetMuscleGroupByIdAsync(int id);
    Task<MuscleGroupResponse> CreateMuscleGroupAsync(CreateMuscleGroupRequest request);
    Task UpdateMuscleGroupAsync(int id, UpdateMuscleGroupRequest request);
    Task DeleteMuscleGroupAsync(int id);
}