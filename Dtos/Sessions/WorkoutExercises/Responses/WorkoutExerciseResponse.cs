using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Responses;

namespace WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Responses
{
	public class WorkoutExerciseResponse
	{
		public Guid Id { get; set; }
		public Guid WorkoutSessionId { get; set; }
		public Guid ExerciseId { get; set; }
		public string ExerciseName { get; set; } = string.Empty;
		public int Order { get; set; }
		public string? Notes { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<ExerciseSetResponse> Sets { get; set; } = [];
	}
}
