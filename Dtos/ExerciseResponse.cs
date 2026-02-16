namespace WorkoutTrackerAPI.Dtos
{
    public class ExerciseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BodyType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
