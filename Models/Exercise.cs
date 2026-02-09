namespace WorkoutTrackerAPI.Models
{
    public class Exercise
    {
        /*
         * - Id (Guid, PK)
         * - Name (string, unique) - "Bench Press", "Squat", etc
         * - BodyType (enum: Abs, Back, Chest, Legs, Shoulders)
         * - Category (enum: Strength, Cardio, Flexibility)
         * - CreatedAt (DateTime)
         */
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BodyType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
