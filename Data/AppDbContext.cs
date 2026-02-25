using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Exercise> Exercises => Set<Exercise>();
        public DbSet<ExerciseCategory> ExerciseCategories => Set<ExerciseCategory>();
        public DbSet<MuscleGroup> MuscleGroups => Set<MuscleGroup>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExerciseCategory>().HasData(
                new ExerciseCategory { Id = 1, Name = "Strength" },
                new ExerciseCategory { Id = 2, Name = "Cardio" },
                new ExerciseCategory { Id = 3, Name = "Mobility" }
            );

            modelBuilder.Entity<MuscleGroup>().HasData(
                new MuscleGroup { Id = 1, Name = "Abs" },
                new MuscleGroup { Id = 2, Name = "Back" },
                new MuscleGroup { Id = 3, Name = "Chest" },
                new MuscleGroup { Id = 4, Name = "Glutes" },
                new MuscleGroup { Id = 5, Name = "Hamstrings" },
                new MuscleGroup { Id = 6, Name = "Quads" },
                new MuscleGroup { Id = 7, Name = "Shoulders" }
            );
        }
    }
}
