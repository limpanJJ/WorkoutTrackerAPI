using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;

namespace WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests
{
	public class CreateWorkoutSessionRequest
	{
		public Guid Id { get; set; }
		public string UserId { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public DateTime StartedAt { get; set; }
		public DateTime? EndedAt { get; set; }
		public string? Notes { get; set; }
		public List<CreateWorkoutExerciseRequest> WorkoutExercises { get; set; } = [];
	}
}
	