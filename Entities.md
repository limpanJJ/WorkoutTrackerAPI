## Entities

### User (extends IdentityUser)

- Id (string, PK) — from ASP.NET Identity
- UserName (string)
- Email (string)
- PasswordHash (string) — managed by Identity
- CreatedAtUtc (DateTimeOffset)

### ExerciseCategory

- Id (int, PK)
- Name (string)

Seeded: Strength (1), Cardio (2), Mobility (3)

### MuscleGroup

- Id (int, PK)
- Name (string)

Seeded: Abs (1), Back (2), Chest (3), Glutes (4), Hamstrings (5), Quads (6), Shoulders (7)

### Exercise

- Id (Guid, PK)
- Name (string)
- CategoryId (int, FK → ExerciseCategory)
- MuscleGroupId (int?, FK → MuscleGroup) — nullable, e.g. cardio has no muscle group
- UserId (string?, FK → User) — null = global default, set = user-created
- CreatedAt (DateTime)

### WorkoutSession

- Id (Guid, PK)
- UserId (string, FK → User)
- Name (string)
- StartedAt (DateTime)
- EndedAt (DateTime?, nullable)
- Notes (string?, nullable)

### WorkoutExercise

- Id (Guid, PK)
- WorkoutSessionId (Guid, FK → WorkoutSession)
- ExerciseId (Guid, FK → Exercise)
- Order (int) — position in session
- Notes (string?, nullable)

### WorkoutExerciseSet

- Id (Guid, PK)
- WorkoutExerciseId (Guid, FK → WorkoutExercise)
- SetNumber (int) — 1, 2, 3, etc.
- Reps (int?, nullable)
- Weight (decimal?, nullable) — kg, precision(18,2)
- DurationSeconds (int?, nullable) — for planks, cardio
- DistanceMeters (decimal?, nullable) — for running, precision(18,2)

## Relationships

- User → WorkoutSessions (1:N)
- User → Exercises (1:N, optional — custom exercises)
- WorkoutSession → WorkoutExercises (1:N)
- Exercise → WorkoutExercises (1:N)
- ExerciseCategory → Exercises (1:N)
- MuscleGroup → Exercises (1:N)
- WorkoutExercise → WorkoutExerciseSets (1:N)
