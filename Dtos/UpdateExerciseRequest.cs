using System;

namespace WorkoutTrackerAPI.Dtos
{
    public class UpdateExerciseRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int? MuscleGroupId { get; set; }
    }
}
