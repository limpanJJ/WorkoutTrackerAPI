namespace WorkoutTrackerAPI.Dtos
{
    public class UpdateExerciseRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BodyType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
