using System.ComponentModel.DataAnnotations;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;

namespace WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests
{
	public class CreateWorkoutSessionRequest
	{
		[Required, MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		[Required]
		public DateTime StartedAt { get; set; }

		public DateTime? EndedAt { get; set; }

		[MaxLength(500)]
		public string? Notes { get; set; }

		public List<CreateWorkoutExerciseRequest> WorkoutExercises { get; set; } = [];
	}
}