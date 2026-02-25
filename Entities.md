
## Entities

### User

- Id (Guid, PK)
- Email (string, unique)
- PasswordHash (string)
- Name (string)
- CreatedAt (DateTime)

### WorkoutSession

- Id (Guid, PK)
- UserId (Guid, FK → User)
- Date (DateTime)
- Notes (string?, nullable)
- CreatedAt (DateTime)

### Exercise (template/library)

- Id (Guid, PK)
- Name (string, unique) - "Bench Press", "Squat", etc
- BodyType (enum: Abs, Back, Chest, Quads, Hamstrings, Glutes, Shoulders) only if Strength
- Category (enum: Strength, Cardio, Flexibility)
- CreatedAt (DateTime)

### SessionExercise (övning I en specifik session)

- Id (Guid, PK)
- WorkoutSessionId (Guid, FK → WorkoutSession)
- ExerciseId (Guid, FK → Exercise)
- Order (int) - vilken ordning i sessionen
- Notes (string?, nullable)
- CreatedAt (DateTime)

### ExerciseSet

- Id (Guid, PK)
- SessionExerciseId (Guid, FK → SessionExercise)
- SetNumber (int) - 1, 2, 3, etc
- Reps (int?, nullable)
- Weight (decimal?, nullable) - kg
- DurationSeconds (int?, nullable) - för planks, cardio
- DistanceMeters (decimal?, nullable) - för löpband
- CreatedAt (DateTime)

## Relationships

- User → WorkoutSessions (1:N)
- WorkoutSession → SessionExercises (1:N)
- Exercise → SessionExercises (1:N) - samma template kan användas i många sessions
- SessionExercise → ExerciseSets (1:N)
