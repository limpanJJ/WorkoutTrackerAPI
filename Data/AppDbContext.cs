using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Exercise> Exercises => Set<Exercise>();
    }
}
