using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Requests;
using WorkoutTrackerAPI.Dtos.Exercises.MuscleGroups.Responses;
using WorkoutTrackerAPI.Exceptions;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Services
{
	public class MuscleGroupService(AppDbContext context) : IMuscleGroupService
	{
		public async Task<MuscleGroupResponse> GetMuscleGroupByIdAsync(int id)
		{
			var muscleGroup = await context.MuscleGroups.FindAsync(id)
				?? throw new NotFoundException($"Muscle group with ID {id} was not found.");
			return MapToResponse(muscleGroup);
		}

		public async Task<List<MuscleGroupResponse>> GetAllMuscleGroupsAsync()
			=> await context.MuscleGroups
				.OrderBy(m => m.Name)
				.Select(m => new MuscleGroupResponse
				{
					Id = m.Id,
					Name = m.Name
				})
				.ToListAsync();

		public async Task UpdateMuscleGroupAsync(int id, UpdateMuscleGroupRequest request)
		{
			var existingMuscleGroup = await context.MuscleGroups.FindAsync(id)
				?? throw new NotFoundException($"Muscle group with ID {id} was not found.");

			await EnsureNameIsUniqueAsync(request.Name, id);

			existingMuscleGroup.Name = request.Name;
			await context.SaveChangesAsync();
		}

		public async Task<MuscleGroupResponse> CreateMuscleGroupAsync(CreateMuscleGroupRequest request)
		{
			await EnsureNameIsUniqueAsync(request.Name);

			var newMuscleGroup = new MuscleGroup
			{
				Name = request.Name
			};

			context.MuscleGroups.Add(newMuscleGroup);
			await context.SaveChangesAsync();

			return MapToResponse(newMuscleGroup);
		}

		public async Task DeleteMuscleGroupAsync(int id)
		{
			var muscleGroup = await context.MuscleGroups.FindAsync(id)
				?? throw new NotFoundException($"Muscle group with ID {id} was not found.");

			context.MuscleGroups.Remove(muscleGroup);
			await context.SaveChangesAsync();
		}

		private async Task EnsureNameIsUniqueAsync(string name, int? excludeId = null)
		{
			var exists = await context.MuscleGroups
				.AnyAsync(m => m.Name == name && (excludeId == null || m.Id != excludeId));

			if (exists)
				throw new ConflictException($"Muscle group '{name}' already exists.");
		}

		private static MuscleGroupResponse MapToResponse(MuscleGroup muscleGroup)
			=> new MuscleGroupResponse
			{
				Id = muscleGroup.Id,
				Name = muscleGroup.Name
			};
	}
}