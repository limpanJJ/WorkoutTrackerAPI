namespace WorkoutTrackerAPI.Dtos
{
    public class CreateExerciseRequest
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int? MuscleGroupId { get; set; }
    }
}
