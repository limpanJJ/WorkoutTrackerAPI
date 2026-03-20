using System;

namespace WorkoutTrackerAPI.Models
{
    /// <summary>
    /// A reusable exercise template defining name, category, and muscle group.
    /// When <see cref="UserId"/> is null the exercise is a global default visible to everyone.
    /// When set, it is a custom exercise owned by that user.
    /// </summary>
    public class Exercise
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public ExerciseCategory Category { get; set; } = null!;

        public int? MuscleGroupId { get; set; }
        public MuscleGroup? MuscleGroup { get; set; }

        /// <summary>
        /// Null = global default exercise. Set = custom exercise owned by this user.
        /// </summary>
        public string? UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
