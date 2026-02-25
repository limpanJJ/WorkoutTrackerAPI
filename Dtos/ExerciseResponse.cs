using System;

namespace WorkoutTrackerAPI.Dtos
{
    public class ExerciseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int? MuscleGroupId { get; set; }
        public string? MuscleGroupName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
