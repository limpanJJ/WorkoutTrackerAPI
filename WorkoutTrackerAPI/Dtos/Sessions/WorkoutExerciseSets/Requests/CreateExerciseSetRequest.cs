using System.ComponentModel.DataAnnotations;

namespace WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests
{
    public class CreateExerciseSetRequest
    {
        [Range(1, int.MaxValue)]
        public int SetNumber { get; set; }

        [Range(0, int.MaxValue)]
        public int? Reps { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Weight { get; set; }

        [Range(0, int.MaxValue)]
        public int? DurationSeconds { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? DistanceMeters { get; set; }
    }
}
