using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Responses;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Responses;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Responses;
using WorkoutTrackerAPI.Exceptions;
using WorkoutTrackerAPI.Models;
using WorkoutTrackerAPI.Repositories;

namespace WorkoutTrackerAPI.Services
{
    public class WorkoutSessionService(IWorkoutSessionRepository repository) : IWorkoutSessionService
    {
        // Workout Sessions
        public Task<List<WorkoutSessionSummaryResponse>> GetAllWorkoutSessionsAsync(string userId)
            => throw new NotImplementedException();

        public async Task<WorkoutSessionResponse> GetWorkoutSessionByIdAsync(Guid id, string userId)
        {
            var session = await repository.GetWorkoutByIdAsync(id, userId)
                ?? throw new NotFoundException($"Workout session with ID '{id}' not found.");

            return MapToResponse(session);
        }

        public async Task<WorkoutSessionResponse> CreateWorkoutSessionAsync(CreateWorkoutSessionRequest request, string userId)
        {
            var session = new WorkoutSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = request.Name,
                StartedAt = request.StartedAt,
                EndedAt = request.EndedAt,
                Notes = request.Notes,
                WorkoutExercises = request.WorkoutExercises.Select((we, index) => new WorkoutExercise
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = we.ExerciseId,
                    Order = index + 1,
                    Notes = we.Notes,
                    WorkoutExerciseSets = we.Sets.Select((s, setIndex) => new WorkoutExerciseSet
                    {
                        Id = Guid.NewGuid(),
                        SetNumber = setIndex + 1,
                        Reps = s.Reps,
                        Weight = s.Weight,
                        DurationSeconds = s.DurationSeconds,
                        DistanceMeters = s.DistanceMeters
                    }).ToList()
                }).ToList()
            };

            await repository.CreateWorkoutAsync(session);
            return MapToResponse(session);
        }

        public Task UpdateWorkoutAsync(Guid id, UpdateWorkoutSessionRequest request, string userId)
            => throw new NotImplementedException();

        public Task DeleteWorkoutSessionAsync(Guid id, string userId)
            => throw new NotImplementedException();

        // Workout Exercises
        public Task AddExerciseToWorkoutAsync(Guid workoutSessionId, CreateWorkoutExerciseRequest request, string userId)
            => throw new NotImplementedException();

        public Task UpdateWorkoutExerciseAsync(Guid workoutSessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request, string userId)
            => throw new NotImplementedException();

        public Task DeleteWorkoutExerciseAsync(Guid workoutSessionId, Guid exerciseId, string userId)
            => throw new NotImplementedException();

        // Exercise Sets
        public Task AddExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, CreateExerciseSetRequest request, string userId)
            => throw new NotImplementedException();

        public Task UpdateExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, UpdateExerciseSetRequest request, string userId)
            => throw new NotImplementedException();

        public Task DeleteExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, string userId)
            => throw new NotImplementedException();

        // Mapping helpers
        private static WorkoutSessionResponse MapToResponse(WorkoutSession session) => new()
        {
            Id = session.Id,
            Name = session.Name,
            StartedAt = session.StartedAt,
            EndedAt = session.EndedAt,
            Notes = session.Notes,
            WorkoutExercises = session.WorkoutExercises.Select(we => new WorkoutExerciseResponse
            {
                Id = we.Id,
                WorkoutSessionId = we.WorkoutSessionId,
                ExerciseId = we.ExerciseId,
                ExerciseName = we.Exercise?.Name ?? string.Empty,
                Order = we.Order,
                Notes = we.Notes,
                Sets = we.WorkoutExerciseSets.Select(s => new ExerciseSetResponse
                {
                    Id = s.Id,
                    WorkoutExerciseId = s.WorkoutExerciseId,
                    SetNumber = s.SetNumber,
                    Reps = s.Reps,
                    Weight = s.Weight,
                    DurationSeconds = s.DurationSeconds,
                    DistanceMeters = s.DistanceMeters
                }).ToList()
            }).ToList()
        };

        private static WorkoutSessionSummaryResponse MapToSummaryResponse(WorkoutSession session) => new()
        {
            Id = session.Id,
            Name = session.Name,
            StartedAt = session.StartedAt,
            EndedAt = session.EndedAt,
            ExerciseCount = session.WorkoutExercises.Count,
            TotalSets = session.WorkoutExercises.Sum(we => we.WorkoutExerciseSets.Count)
        };
    }
}
