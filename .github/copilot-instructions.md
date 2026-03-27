# Copilot Instructions

## Project Guidelines

### Project Overview
- WorkoutTracker API is a RESTful API developed in .NET 10 with Identity + JWT-auth and EF Core + SQL Server, designed to manage workout sessions, exercises, and user progress.
- Services, controllers, and data access layers are categorized by feature (e.g., Auth, Exercises, WorkoutSessions).
- DTOs are organized in Requests and Responses subfolders by feature.

### Routing
- Uses PascalCase in code, automatically converted to kebab-case via SlugifyParameterTransformer. For example, `WorkoutSession` becomes `/workout-sessions`.
- Never create routes with camelCase or snake_case; always use PascalCase and let the transformer handle it.

### Config
- Configuration through `appsettings.json` and environment variables, with secrets loaded from a `.env` file (not committed to git).

### Authentication
- JWT-based authentication with ASP.NET Identity for user management.
- Endpoints requiring authentication are protected with the `[Authorize]` attribute.
- In development, a demo-admin is used for testing. In production, real user registration and login is required.

### Data Access
- Uses Entity Framework Core with SQL Server.
- Entities: User, ExerciseCategory, MuscleGroup, Exercise, WorkoutSession, WorkoutExercise, WorkoutExerciseSet.
- EF Core migrations manage database schema changes.
- Seed data is provided for exercise categories and muscle groups.

### Error Handling
- Never throw built-in .NET exceptions (KeyNotFoundException, InvalidOperationException, etc.) to signal HTTP errors. Always use custom exceptions from the Exceptions folder.
- Custom exceptions are thrown in the service layer when business rules are violated:
  - `NotFoundException` → 404 Not Found
  - `ConflictException` → 409 Conflict
  - `UnauthorizedException` → 401 Unauthorized
- `ExceptionHandlingMiddleware` catches custom exceptions and returns a `ProblemDetails` response. Unhandled exceptions are logged and return a generic 500 Internal Server Error.
- Do not create custom exceptions for validation errors; return 400 Bad Request with error details instead.

### API Documentation
- OpenAPI with Scalar UI, accessible at `/scalar`.