using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkoutTrackerAPI.Constants;
using WorkoutTrackerAPI.Models;

namespace WorkoutTrackerAPI.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // ---- Seed roles ----
        string[] roles = [Roles.Admin, Roles.User];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

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
            else
            {
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
        }

        // ---- Seed demo exercises ----
        const int strengthCategoryId = 1;
        const int cardioCategoryId = 2;
        const int mobilityCategoryId = 3;

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
            new Exercise { Id = Guid.NewGuid(), Name = "Outdoor Running", CategoryId = cardioCategoryId,   MuscleGroupId = null,     CreatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), Name = "Runner's Lunge",  CategoryId = mobilityCategoryId, MuscleGroupId = glutesId, CreatedAt = DateTime.UtcNow },
        };

        foreach (var ex in demoExercises)
        {
            var exists = await context.Exercises.AnyAsync(e => e.Name == ex.Name);
            if (!exists)
            {
                context.Exercises.Add(ex);
            }
        }

        await context.SaveChangesAsync();

        // ---- Seed demo workout sessions ----
        if (await context.WorkoutSessions.AnyAsync(ws => ws.UserId == user.Id))
            return;

        // Reload exercises from DB so we get stable IDs
        var pushUp = await context.Exercises.FirstAsync(e => e.Name == "Push-up");
        var squats = await context.Exercises.FirstAsync(e => e.Name == "Squats");
        var pullUp = await context.Exercises.FirstAsync(e => e.Name == "Pull-up");
        var running = await context.Exercises.FirstAsync(e => e.Name == "Outdoor Running");
        var lunge = await context.Exercises.FirstAsync(e => e.Name == "Runner's Lunge");

        var now = DateTime.UtcNow;

        // Session 1: Strength
        var session1 = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Mixed Upper Body",
            StartedAt = now.AddDays(-3),
            EndedAt = now.AddDays(-3).AddMinutes(45),
            Notes = "Felt week today, but pushed through!",
            WorkoutExercises = new List<WorkoutExercise>
           {
               new()
               {
                   Id = Guid.NewGuid(),
                   ExerciseId = pushUp.Id,
                   Order = 1,
                   WorkoutExerciseSets = new List<WorkoutExerciseSet>
                   {
                       new() { Id = Guid.NewGuid(), SetNumber = 1, Reps = 10 },
                       new() { Id = Guid.NewGuid(), SetNumber = 2, Reps = 8 },
                       new() { Id = Guid.NewGuid(), SetNumber = 3, Reps = 6 }
                   }
               },
               new()
               {
                   Id = Guid.NewGuid(),
                   ExerciseId = squats.Id,
                   Order = 2,
                   WorkoutExerciseSets = new List<WorkoutExerciseSet>
                   {
                       new() { Id = Guid.NewGuid(), SetNumber = 1, Reps = 12, Weight = 100 },
                       new() { Id = Guid.NewGuid(), SetNumber = 2, Reps = 12, Weight = 100 },
                       new() { Id = Guid.NewGuid(), SetNumber = 3, Reps = 12, Weight = 100 }
                   }
               },
               new()
               {
                   Id = Guid.NewGuid(),
                   ExerciseId = pullUp.Id,
                   Order = 3,
                   WorkoutExerciseSets = new List<WorkoutExerciseSet>
                   {
                       new() { Id = Guid.NewGuid(), SetNumber = 1, Reps = 8 },
                       new() { Id = Guid.NewGuid(), SetNumber = 2, Reps = 8 },
                       new() { Id = Guid.NewGuid(), SetNumber = 3, Reps = 8 },
                       new() { Id = Guid.NewGuid(), SetNumber = 4, Reps = 7 }
                   }
               }
           }
        };

        // Session 2: Cardio + Mobility
        var session2 = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Name = "Relaxed Run",
            StartedAt = now.AddDays(-1),
            EndedAt = now.AddDays(-1).AddHours(1).AddMinutes(30),
            WorkoutExercises = new List<WorkoutExercise>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = lunge.Id,
                    Order = 1,
                    WorkoutExerciseSets = new List<WorkoutExerciseSet>
                    {
                        new() { Id = Guid.NewGuid(), SetNumber = 1, Reps = 10 },
                        new() { Id = Guid.NewGuid(), SetNumber = 2, Reps = 10 },
                        new() { Id = Guid.NewGuid(), SetNumber = 3, Reps = 10 }
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = running.Id,
                    Order = 2,
                    WorkoutExerciseSets = new List<WorkoutExerciseSet>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            SetNumber = 1,
                            DurationSeconds = 3738,
                            DistanceMeters = 10000
                        }
                    }
                }
            },
        };

        context.WorkoutSessions.AddRange(session1, session2);
        await context.SaveChangesAsync();

    }
}