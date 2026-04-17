using NSubstitute;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Models;
using WorkoutTrackerAPI.Repositories;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Tests
{
    public class WorkoutSessionTests
    {
        private readonly WorkoutSessionService _sut;
        private readonly IWorkoutSessionRepository _workoutSessionRepository = Substitute.For<IWorkoutSessionRepository>();

        public WorkoutSessionTests()
        {
            _sut = new WorkoutSessionService(_workoutSessionRepository);
        }

        // Test for GetAllWorkoutsAsync
        [Fact]
        public async Task GetAllWorkoutSessionsAsync_ShouldReturnCorrectlyMappedSummaryResponses()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var sessions = new List<WorkoutSession>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Push Day",
                    StartedAt = DateTime.UtcNow,
                    EndedAt = DateTime.UtcNow.AddHours(1),
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            ExerciseId = Guid.NewGuid(),
                            WorkoutExerciseSets = new List<WorkoutExerciseSet>
                            {
                                new() { Id = Guid.NewGuid(), Reps = 10, Weight = 80 },
                                new() { Id = Guid.NewGuid(), Reps = 8, Weight = 85 }
                            }
                        }
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = "Leg Day",
                    StartedAt = DateTime.UtcNow,
                    EndedAt = null,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            ExerciseId = Guid.NewGuid(),
                            WorkoutExerciseSets = new List<WorkoutExerciseSet>
                            {
                                new() { Id = Guid.NewGuid(), Reps = 5, Weight = 120 }
                            }
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            ExerciseId = Guid.NewGuid(),
                            WorkoutExerciseSets = new List<WorkoutExerciseSet>
                            {
                                new() { Id = Guid.NewGuid(), Reps = 12, Weight = 60 },
                                new() { Id = Guid.NewGuid(), Reps = 10, Weight = 65 },
                                new() { Id = Guid.NewGuid(), Reps = 8, Weight = 70 }
                            }
                        }
                    }
                }
            };

            _workoutSessionRepository
                .GetAllWorkoutsAsync(userId)
                .Returns(sessions);

            // Act
            var result = await _sut.GetAllWorkoutSessionsAsync(userId);

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal("Push Day", result[0].Name);
            Assert.Equal(1, result[0].ExerciseCount);
            Assert.Equal(2, result[0].TotalSets);

            Assert.Equal("Leg Day", result[1].Name);
            Assert.Null(result[1].EndedAt);
            Assert.Equal(2, result[1].ExerciseCount);
            Assert.Equal(4, result[1].TotalSets);
        }

        [Fact]
        public async Task GetAllWorkoutSessionsAsync_ShouldReturnEmptyList_WhenNoWorkoutSessionsExist()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _workoutSessionRepository
                .GetAllWorkoutsAsync(userId)
                .Returns(new List<WorkoutSession>());
            // Act
            var result = await _sut.GetAllWorkoutSessionsAsync(userId);
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // Test for GetWorkoutSessionByIdAsync
        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldReturnWorkoutSession_WhenWorkoutSessionExists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var workoutSessionId = Guid.NewGuid();
            var session = CreateStandardWorkoutEntity(workoutSessionId, userId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(workoutSessionId, userId, false)
                .Returns(session);

            // Act
            var result = await _sut.GetWorkoutSessionByIdAsync(workoutSessionId, userId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(workoutSessionId, result.Id);
            Assert.Equal("Workout Session 1", result.Name);
            Assert.Equal("Felt great!", result.Notes);
            Assert.Single(result.WorkoutExercises);
            Assert.Equal(10, result.WorkoutExercises[0].Sets[0].Reps);
        }

        // Test for CreateWorkoutSessionAsync
        [Fact]
        public async Task CreateWorkoutSessionAsync_ShouldCreateWorkoutSession()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new CreateWorkoutSessionRequest
            {
                Name = "Workout Session 1",
                StartedAt = DateTime.UtcNow,
                EndedAt = DateTime.UtcNow.AddHours(1),
                Notes = "Felt great!",
                WorkoutExercises =
                [
                    new CreateWorkoutExerciseRequest
                    {
                        ExerciseId = Guid.NewGuid(),
                        Sets =
                        [
                            new CreateExerciseSetRequest
                            {
                                Reps = 10,
                                Weight = 100
                            }
                        ]
                    }
                ]
            };

            _workoutSessionRepository
                .CreateWorkoutAsync(Arg.Any<WorkoutSession>())
                .Returns(callInfo => callInfo.Arg<WorkoutSession>());

            // Act
            var result = await _sut.CreateWorkoutSessionAsync(request, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Workout Session 1", result.Name);
            Assert.Equal("Felt great!", result.Notes);
            Assert.Single(result.WorkoutExercises);
            Assert.Equal(10, result.WorkoutExercises[0].Sets[0].Reps);
        }
        private static WorkoutSession CreateStandardWorkoutEntity(Guid workoutSessionId, string userId)
            => new()
            {
                Id = workoutSessionId,
                UserId = userId,
                Name = "Workout Session 1",
                StartedAt = DateTime.UtcNow,
                EndedAt = DateTime.UtcNow.AddHours(1),
                Notes = "Felt great!",
                WorkoutExercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise
                    {
                        Id = Guid.NewGuid(),
                        ExerciseId = Guid.NewGuid(),
                        WorkoutExerciseSets = new List<WorkoutExerciseSet>
                        {
                            new WorkoutExerciseSet
                            {
                                Id = Guid.NewGuid(),
                                Reps = 10,
                                Weight = 100
                            }
                        }
                    }
                }
            };

    }
}
