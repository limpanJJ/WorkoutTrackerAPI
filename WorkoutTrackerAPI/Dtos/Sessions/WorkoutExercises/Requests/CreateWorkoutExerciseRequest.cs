using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;

namespace WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests
{
    public class CreateWorkoutExerciseRequest
    {
        public Guid ExerciseId { get; set; }
        public int Order { get; set; }
        public string? Notes { get; set; }
        public List<CreateExerciseSetRequest>? Sets { get; set; }
    }
}
