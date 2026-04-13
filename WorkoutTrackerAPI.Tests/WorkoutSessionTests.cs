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
