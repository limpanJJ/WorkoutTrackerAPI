using System;

namespace WorkoutTrackerAPI.Models
{
    /// <summary>
    /// A reusable exercise template defining name, category, and muscle group.
    /// Not tied to any specific session — see <see cref="SessionExercise"/> for session-specific usage.
    /// </summary>
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
