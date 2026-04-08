using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Responses;

namespace WorkoutTrackerAPI.Dtos.Sessions.Workouts.Responses
{
    public class WorkoutSessionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string? Notes { get; set; }
        public List<WorkoutExerciseResponse> WorkoutExercises { get; set; } = [];
    }
}
