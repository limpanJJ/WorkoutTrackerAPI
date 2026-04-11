namespace WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests
{
    public class UpdateExerciseSetRequest
    {
        public int SetNumber { get; set; }
        public int? Reps { get; set; }
        public decimal? Weight { get; set; }
        public int? DurationSeconds { get; set; }
        public decimal? DistanceMeters { get; set; }
    }
}
