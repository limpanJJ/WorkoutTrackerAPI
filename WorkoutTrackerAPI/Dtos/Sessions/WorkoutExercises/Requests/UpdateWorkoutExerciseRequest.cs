namespace WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests
{
	public class UpdateWorkoutExerciseRequest
	{
		public int Order { get; set; }
		public string? Notes { get; set; }
	}
}
