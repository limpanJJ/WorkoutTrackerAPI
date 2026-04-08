namespace WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Responses
{
    public class ExerciseSetResponse
    {
        public Guid Id { get; set; }
        public Guid WorkoutExerciseId { get; set; }
        public int SetNumber { get; set; }
        public int? Reps { get; set; }
        public decimal? Weight { get; set; }
        public int? DurationSeconds { get; set; }
        public decimal? DistanceMeters { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
