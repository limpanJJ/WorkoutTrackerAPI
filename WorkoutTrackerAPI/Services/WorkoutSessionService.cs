using WorkoutTrackerAPI.Dtos.Common;
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
        public async Task<PagedResponse<WorkoutSessionSummaryResponse>> GetAllWorkoutSessionsAsync(string userId, int page, int pageSize)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var totalCount = await repository.CountWorkoutsAsync(userId);
            var sessions = await repository.GetAllWorkoutsAsync(userId, page, pageSize);

            return new PagedResponse<WorkoutSessionSummaryResponse>
            {
                Items = sessions.Select(MapToSummaryResponse).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<WorkoutSessionResponse> GetWorkoutSessionByIdAsync(Guid id, string userId)
        {
            var session = await GetSessionOrThrowAsync(id, userId, tracked: false);
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

        public async Task UpdateWorkoutAsync(Guid id, UpdateWorkoutSessionRequest request, string userId)
        {
            var session = await GetSessionOrThrowAsync(id, userId);

            session.Name = request.Name;
            session.StartedAt = request.StartedAt;
            session.EndedAt = request.EndedAt;
            session.Notes = request.Notes;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteWorkoutSessionAsync(Guid id, string userId)
        {
            var session = await GetSessionOrThrowAsync(id, userId);
            await repository.DeleteWorkoutAsync(session);
        }

        // Workout Exercises
        public async Task AddExerciseToWorkoutAsync(Guid workoutSessionId, CreateWorkoutExerciseRequest request, string userId)
        {
            await GetSessionOrThrowAsync(workoutSessionId, userId);

            var workoutExercise = new WorkoutExercise
            {
                ExerciseId = request.ExerciseId,
                WorkoutSessionId = workoutSessionId,
                Order = request.Order,
                Notes = request.Notes,
                WorkoutExerciseSets = request.Sets.Select((s, i) => new WorkoutExerciseSet
                {
                    SetNumber = i + 1,
                    Reps = s.Reps,
                    Weight = s.Weight,
                    DurationSeconds = s.DurationSeconds,
                    DistanceMeters = s.DistanceMeters
                }).ToList()
            };

            await repository.AddWorkoutExerciseAsync(workoutExercise);
        }

        public async Task UpdateWorkoutExerciseAsync(Guid workoutSessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request, string userId)
        {
            var session = await GetSessionOrThrowAsync(workoutSessionId, userId);
            var workoutExercise = GetExerciseOrThrow(session, exerciseId);

            workoutExercise.Order = request.Order;
            workoutExercise.Notes = request.Notes;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteWorkoutExerciseAsync(Guid workoutSessionId, Guid exerciseId, string userId)
        {
            var session = await GetSessionOrThrowAsync(workoutSessionId, userId);
            var workoutExercise = GetExerciseOrThrow(session, exerciseId);
            await repository.DeleteWorkoutExerciseAsync(workoutExercise);
        }

        // Exercise Sets
        public async Task AddExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, CreateExerciseSetRequest request, string userId)
        {
            var session = await GetSessionOrThrowAsync(workoutSessionId, userId);
            var workoutExercise = GetExerciseOrThrow(session, exerciseId);

            var exerciseSet = new WorkoutExerciseSet
            {
                WorkoutExerciseId = exerciseId,
                SetNumber = request.SetNumber,
                Reps = request.Reps,
                Weight = request.Weight,
                DurationSeconds = request.DurationSeconds,
                DistanceMeters = request.DistanceMeters
            };

            await repository.AddExerciseSetAsync(exerciseSet);
        }

        public async Task UpdateExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, UpdateExerciseSetRequest request, string userId)
        {
            var session = await GetSessionOrThrowAsync(workoutSessionId, userId);
            var workoutExercise = GetExerciseOrThrow(session, exerciseId);

            var exerciseSet = workoutExercise.WorkoutExerciseSets.FirstOrDefault(s => s.Id == setId)
                ?? throw new NotFoundException($"Exercise set with ID '{setId}' not found in workout exercise '{exerciseId}'.");

            exerciseSet.SetNumber = request.SetNumber;
            exerciseSet.Reps = request.Reps;
            exerciseSet.Weight = request.Weight;
            exerciseSet.DurationSeconds = request.DurationSeconds;
            exerciseSet.DistanceMeters = request.DistanceMeters;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteExerciseSetAsync(Guid workoutSessionId, Guid exerciseId, Guid setId, string userId)
        {
            var session = await GetSessionOrThrowAsync(workoutSessionId, userId);
            var workoutExercise = GetExerciseOrThrow(session, exerciseId);

            var exerciseSet = workoutExercise.WorkoutExerciseSets.FirstOrDefault(s => s.Id == setId)
                ?? throw new NotFoundException($"Exercise set with ID '{setId}' not found in workout exercise '{exerciseId}'.");

            await repository.DeleteExerciseSetAsync(exerciseSet);
        }

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

        private async Task<WorkoutSession> GetSessionOrThrowAsync(Guid workoutSessionId, string userId, bool tracked = true)
        {
            var session = await repository.GetWorkoutByIdAsync(workoutSessionId, userId, tracked)
                ?? throw new NotFoundException($"Workout session with ID '{workoutSessionId}' not found.");

            if (session.UserId != userId)
                throw new UnauthorizedException("You do not have permission to modify this workout session.");

            return session;
        }

        private static WorkoutExercise GetExerciseOrThrow(WorkoutSession session, Guid exerciseId)
        {
            return session.WorkoutExercises.FirstOrDefault(we => we.Id == exerciseId)
                ?? throw new NotFoundException($"Workout exercise with ID '{exerciseId}' not found in session '{session.Id}'.");
        }
    }
}
