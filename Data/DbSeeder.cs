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
        const int strengthCategoryId = 1;
        const int cardioCategoryId   = 2;
        const int mobilityCategoryId = 3;

        const int absId    = 1;
        const int backId   = 2;
        const int chestId  = 3;
        const int glutesId = 4;
        const int quadsId  = 6;

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
                context.Exercises.Add(ex);
        }

        await context.SaveChangesAsync();

        // ---- Seed workout sessions ----
        var hasWorkoutSessions = await context.WorkoutSessions.AnyAsync(ws => ws.UserId == user.Id);
        if (hasWorkoutSessions)
            return;

        var pushUp       = await context.Exercises.FirstAsync(e => e.Name == "Push-up");
        var squats       = await context.Exercises.FirstAsync(e => e.Name == "Squats");
        var pullUp       = await context.Exercises.FirstAsync(e => e.Name == "Pull-up");
        var running      = await context.Exercises.FirstAsync(e => e.Name == "Outdoor Running");
        var runnersLunge = await context.Exercises.FirstAsync(e => e.Name == "Runner's Lunge");

        var now = DateTime.UtcNow;

        var sessions = new List<WorkoutSession>
        {
            // Session 1 (7 days ago): Upper Body Strength
            BuildSession(user.Id, "Upper Body Strength", now.AddDays(-7), now.AddDays(-7).AddMinutes(45),
                notes: "Felt good, focused on chest and back form.",
                exercises:
                [
                    BuildSessionExercise(pushUp.Id, order: 1, notes: null, sets:
                    [
                        BuildSet(1, reps: 12),
                        BuildSet(2, reps: 10),
                        BuildSet(3, reps: 10),
                        BuildSet(4, reps: 8),
                    ]),
                    BuildSessionExercise(pullUp.Id, order: 2, notes: "Controlled negatives on last set.", sets:
                    [
                        BuildSet(1, reps: 8),
                        BuildSet(2, reps: 7),
                        BuildSet(3, reps: 6),
                    ]),
                ]),

            // Session 2 (5 days ago): Cardio
            BuildSession(user.Id, "Morning Cardio Run", now.AddDays(-5), now.AddDays(-5).AddMinutes(38),
                notes: "Morning run. Nice weather, steady pace throughout.",
                exercises:
                [
                    BuildSessionExercise(running.Id, order: 1, notes: "Interval run — fast, moderate, cool-down.", sets:
                    [
                        BuildSet(1, durationSeconds: 900,  distanceMeters: 2500m),
                        BuildSet(2, durationSeconds: 600,  distanceMeters: 1500m),
                        BuildSet(3, durationSeconds: 360,  distanceMeters: 800m),
                    ]),
                ]),

            // Session 3 (3 days ago): Legs + Mobility
            BuildSession(user.Id, "Legs + Mobility", now.AddDays(-3), now.AddDays(-3).AddMinutes(50),
                notes: "Legs were burning. Added a bit of weight on later sets.",
                exercises:
                [
                    BuildSessionExercise(squats.Id, order: 1, notes: "Progressed from bodyweight to 20 kg.", sets:
                    [
                        BuildSet(1, reps: 15, weight: 0m),
                        BuildSet(2, reps: 12, weight: 0m),
                        BuildSet(3, reps: 10, weight: 20m),
                        BuildSet(4, reps: 10, weight: 20m),
                    ]),
                    BuildSessionExercise(runnersLunge.Id, order: 2, notes: "Hip flexors were really tight today.", sets:
                    [
                        BuildSet(1, durationSeconds: 30),
                        BuildSet(2, durationSeconds: 30),
                    ]),
                ]),

            // Session 4 (yesterday): Full Body
            BuildSession(user.Id, "Full Body Session", now.AddDays(-1), now.AddDays(-1).AddMinutes(55),
                notes: null,
                exercises:
                [
                    BuildSessionExercise(pushUp.Id, order: 1, notes: null, sets:
                    [
                        BuildSet(1, reps: 15),
                        BuildSet(2, reps: 12),
                        BuildSet(3, reps: 10),
                    ]),
                    BuildSessionExercise(squats.Id, order: 2, notes: null, sets:
                    [
                        BuildSet(1, reps: 12, weight: 20m),
                        BuildSet(2, reps: 10, weight: 20m),
                        BuildSet(3, reps: 8,  weight: 25m),
                    ]),
                    BuildSessionExercise(pullUp.Id, order: 3, notes: "Getting stronger — best set yet!", sets:
                    [
                        BuildSet(1, reps: 9),
                        BuildSet(2, reps: 7),
                        BuildSet(3, reps: 6),
                    ]),
                ]),
        };

        context.WorkoutSessions.AddRange(sessions);
        await context.SaveChangesAsync();
    }

    // ---- Helpers ----

    private static WorkoutSession BuildSession(
        string userId, string name, DateTime startedAt, DateTime endedAt,
        string? notes, WorkoutExercise[] exercises) 
    {
        var session = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name,
            StartedAt = startedAt,
            EndedAt = endedAt,
            Notes = notes,
            WorkoutExercises = exercises
        };

        foreach (var ex in exercises)
            ex.WorkoutSessionId = session.Id;

        return session;
    }

    private static WorkoutExercise BuildSessionExercise(
        Guid exerciseId, int order, string? notes, WorkoutExerciseSet[] sets)
    {
        var sessionExercise = new WorkoutExercise
        {
            Id = Guid.NewGuid(),
            ExerciseId = exerciseId,
            Order = order,
            Notes = notes,
            CreatedAt = DateTime.UtcNow,
            WorkoutExerciseSets = sets
        };

        foreach (var set in sets)
            set.WorkoutExerciseId = sessionExercise.Id;

        return sessionExercise;
    }

    private static WorkoutExerciseSet BuildSet(
        int setNumber,
        int? reps = null,
        decimal? weight = null,
        int? durationSeconds = null,
        decimal? distanceMeters = null) =>
        new()
        {
            Id = Guid.NewGuid(),
            SetNumber = setNumber,
            Reps = reps,
            Weight = weight,
            DurationSeconds = durationSeconds,
            DistanceMeters = distanceMeters,
            CreatedAt = DateTime.UtcNow
        };
}