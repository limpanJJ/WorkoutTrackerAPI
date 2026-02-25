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
    }
}
