using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Responses;
using WorkoutTrackerAPI.Repositories;

namespace WorkoutTrackerAPI.Services
{
	public class WorkoutSessionService(IWorkoutSessionRepository repository) : IWorkoutSessionService
	{
		// Workout Sessions
		public Task<List<WorkoutSessionSummaryResponse>> GetAllWorkoutSessionsAsync(string userId)
			=> throw new NotImplementedException();

		public Task<WorkoutSessionResponse> GetWorkoutSessionByIdAsync(Guid id, string userId)
			=> throw new NotImplementedException();

		public Task<WorkoutSessionResponse> CreateWorkoutSessionAsync(CreateWorkoutSessionRequest request, string userId)
			=> throw new NotImplementedException();

		public Task UpdateWorkoutAsync(Guid id, UpdateWorkoutSessionRequest request, string userId)
			=> throw new NotImplementedException();

		public Task DeleteWorkoutSessionAsync(Guid id, string userId)
			=> throw new NotImplementedException();

		// Workout Exercises
		public Task AddExerciseToWorkoutAsync(Guid workoutSessionId, CreateWorkoutExerciseRequest request, string userId)
			=> throw new NotImplementedException();

		public Task UpdateWorkoutExerciseAsync(Guid workoutSessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request, string userId)
			=> throw new NotImplementedException();

		public Task DeleteWorkoutExerciseAsync(Guid workoutSessionId, Guid exerciseId, string userId)
			=> throw new NotImplementedException();

		// Exercise Sets
		public Task AddExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, CreateExerciseSetRequest request, string userId)
			=> throw new NotImplementedException();

		public Task UpdateExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, UpdateExerciseSetRequest request, string userId)
			=> throw new NotImplementedException();

		public Task DeleteExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, string userId)
			=> throw new NotImplementedException();
	}
}
