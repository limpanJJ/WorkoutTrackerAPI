namespace WorkoutTrackerAPI.Dtos.Sessions.Workouts.Responses
{
    public class WorkoutSessionSummaryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int ExerciseCount { get; set; }
        public int TotalSets { get; set; }
    }
}
