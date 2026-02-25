using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        // ---- Seed demo user (Identity) ----
        const string email = "john.doe@example.com";
        const string password = "JohnDoe123!";

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new User
            {
                UserName = "john.doe",
                Email = email,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                logger.LogWarning("Failed to seed demo user: {Errors}", errors);
            }
        }

        // ---- Seed demo exercises ----
        // Categories (from your HasData):
        const int strengthCategoryId = 1;
        const int cardioCategoryId = 2;
        const int mobilityCategoryId = 3;

        // MuscleGroups (from your HasData):
        const int absId = 1;
        const int backId = 2;
        const int chestId = 3;
        const int glutesId = 4;
        const int quadsId = 6;

        var demoExercises = new[]
        {
            new Exercise { Id = Guid.NewGuid(), Name = "Push-up",         CategoryId = strengthCategoryId, MuscleGroupId = chestId,  CreatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), Name = "Squats",          CategoryId = strengthCategoryId, MuscleGroupId = quadsId,  CreatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), Name = "Pull-up",         CategoryId = strengthCategoryId, MuscleGroupId = backId,   CreatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), Name = "Outdoor Running", CategoryId = cardioCategoryId,    MuscleGroupId = null,     CreatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), Name = "Runner's Lunge",  CategoryId = mobilityCategoryId,  MuscleGroupId = glutesId, CreatedAt = DateTime.UtcNow },
        };

        foreach (var ex in demoExercises)
        {
            var exists = await context.Exercises.AnyAsync(e => e.Name == ex.Name);
            if (!exists)
                context.Exercises.Add(ex);
        }

        await context.SaveChangesAsync();
    }
}