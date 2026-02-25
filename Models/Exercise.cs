using System;

namespace WorkoutTrackerAPI.Models
{
    public class Exercise
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public ExerciseCategory Category { get; set; } = null!;

        public int? MuscleGroupId { get; set; }
        public MuscleGroup? MuscleGroup { get; set; }
        
        public DateTime CreatedAt { get; set; }

    }
}
