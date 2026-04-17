using NSubstitute;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExerciseSets.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.WorkoutExercises.Requests;
using WorkoutTrackerAPI.Dtos.Sessions.Workouts.Requests;
using WorkoutTrackerAPI.Exceptions;
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

        #region GetAllWorkoutSessionsAsync

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

        #endregion

        #region GetWorkoutSessionByIdAsync

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

        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldThrowNotFoundException_WhenWorkoutSessionDoesNotExist()
        {
            // Arrange
            _workoutSessionRepository
                .GetWorkoutByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), false)
                .Returns((WorkoutSession?)null);

            // Act
            var act = () => _sut.GetWorkoutSessionByIdAsync(Guid.NewGuid(), Guid.NewGuid().ToString());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task GetWorkoutByIdAsync_ShouldThrowUnauthorizedException_WhenUserDoesNotOwnSession()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var sessionId = Guid.NewGuid();
            var session = CreateStandardWorkoutEntity(sessionId, Guid.NewGuid().ToString());

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, false)
                .Returns(session);

            // Act
            var act = () => _sut.GetWorkoutSessionByIdAsync(sessionId, userId);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(act);
        }

        #endregion

        #region CreateWorkoutSessionAsync

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

        #endregion

        #region UpdateWorkoutAsync

        [Fact]
        public async Task UpdateWorkoutAsync_ShouldUpdateSession_WhenSessionExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntity(sessionId, userId);
            var request = new UpdateWorkoutSessionRequest
            {
                Name = "Updated Name",
                StartedAt = DateTime.UtcNow,
                EndedAt = DateTime.UtcNow.AddHours(2),
                Notes = "Updated notes"
            };

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            await _sut.UpdateWorkoutAsync(sessionId, request, userId);

            // Assert
            Assert.Equal("Updated Name", session.Name);
            Assert.Equal("Updated notes", session.Notes);
            await _workoutSessionRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateWorkoutAsync_ShouldThrowNotFoundException_WhenSessionDoesNotExist()
        {
            // Arrange
            _workoutSessionRepository
                .GetWorkoutByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), true)
                .Returns((WorkoutSession?)null);

            // Act
            var act = () => _sut.UpdateWorkoutAsync(Guid.NewGuid(), new UpdateWorkoutSessionRequest(), Guid.NewGuid().ToString());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _workoutSessionRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateWorkoutAsync_ShouldThrowUnauthorizedException_WhenUserDoesNotOwnSession()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntity(sessionId, Guid.NewGuid().ToString());

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            var act = () => _sut.UpdateWorkoutAsync(sessionId, new UpdateWorkoutSessionRequest(), userId);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(act);
            await _workoutSessionRepository.DidNotReceive().SaveChangesAsync();
        }

        #endregion

        #region DeleteWorkoutSessionAsync

        [Fact]
        public async Task DeleteWorkoutSessionAsync_ShouldDeleteSession_WhenSessionExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntity(sessionId, userId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            await _sut.DeleteWorkoutSessionAsync(sessionId, userId);

            // Assert
            await _workoutSessionRepository.Received(1).DeleteWorkoutAsync(session);
        }

        [Fact]
        public async Task DeleteWorkoutSessionAsync_ShouldThrowNotFoundException_WhenSessionDoesNotExist()
        {
            // Arrange
            _workoutSessionRepository
                .GetWorkoutByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), true)
                .Returns((WorkoutSession?)null);

            // Act
            var act = () => _sut.DeleteWorkoutSessionAsync(Guid.NewGuid(), Guid.NewGuid().ToString());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _workoutSessionRepository.DidNotReceive().DeleteWorkoutAsync(Arg.Any<WorkoutSession>());
        }

        [Fact]
        public async Task DeleteWorkoutSessionAsync_ShouldThrowUnauthorizedException_WhenUserDoesNotOwnSession()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntity(sessionId, Guid.NewGuid().ToString());

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            var act = () => _sut.DeleteWorkoutSessionAsync(sessionId, userId);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(act);
            await _workoutSessionRepository.DidNotReceive().DeleteWorkoutAsync(Arg.Any<WorkoutSession>());
        }

        #endregion

        #region AddExerciseToWorkoutAsync

        [Fact]
        public async Task AddExerciseToWorkoutAsync_ShouldAddExercise_WhenSessionExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntity(sessionId, userId);
            var request = new CreateWorkoutExerciseRequest
            {
                ExerciseId = Guid.NewGuid(),
                Order = 2,
                Notes = "Go heavy",
                Sets = [new CreateExerciseSetRequest { SetNumber = 1, Reps = 8, Weight = 100 }]
            };

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            await _sut.AddExerciseToWorkoutAsync(sessionId, request, userId);

            // Assert
            await _workoutSessionRepository.Received(1).AddWorkoutExerciseAsync(Arg.Is<WorkoutExercise>(we =>
                we.ExerciseId == request.ExerciseId &&
                we.WorkoutSessionId == sessionId &&
                we.Order == 2
            ));
        }

        [Fact]
        public async Task AddExerciseToWorkoutAsync_ShouldThrowNotFoundException_WhenSessionDoesNotExist()
        {
            // Arrange
            _workoutSessionRepository
                .GetWorkoutByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), true)
                .Returns((WorkoutSession?)null);

            // Act
            var act = () => _sut.AddExerciseToWorkoutAsync(Guid.NewGuid(), new CreateWorkoutExerciseRequest { Sets = [] }, Guid.NewGuid().ToString());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _workoutSessionRepository.DidNotReceive().AddWorkoutExerciseAsync(Arg.Any<WorkoutExercise>());
        }

        #endregion

        #region UpdateWorkoutExerciseAsync

        [Fact]
        public async Task UpdateWorkoutExerciseAsync_ShouldUpdateExercise_WhenExerciseExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntityWithExerciseId(sessionId, userId, exerciseId);
            var request = new UpdateWorkoutExerciseRequest { Order = 5, Notes = "Updated" };

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            await _sut.UpdateWorkoutExerciseAsync(sessionId, exerciseId, request, userId);

            // Assert
            var exercise = session.WorkoutExercises.First(we => we.Id == exerciseId);
            Assert.Equal(5, exercise.Order);
            Assert.Equal("Updated", exercise.Notes);
            await _workoutSessionRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateWorkoutExerciseAsync_ShouldThrowNotFoundException_WhenExerciseDoesNotExist()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntity(sessionId, userId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            var act = () => _sut.UpdateWorkoutExerciseAsync(sessionId, Guid.NewGuid(), new UpdateWorkoutExerciseRequest(), userId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _workoutSessionRepository.DidNotReceive().SaveChangesAsync();
        }

        #endregion

        #region DeleteWorkoutExerciseAsync

        [Fact]
        public async Task DeleteWorkoutExerciseAsync_ShouldDeleteExercise_WhenExerciseExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntityWithExerciseId(sessionId, userId, exerciseId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            await _sut.DeleteWorkoutExerciseAsync(sessionId, exerciseId, userId);

            // Assert
            await _workoutSessionRepository.Received(1).DeleteWorkoutExerciseAsync(Arg.Is<WorkoutExercise>(we => we.Id == exerciseId));
        }

        [Fact]
        public async Task DeleteWorkoutExerciseAsync_ShouldThrowNotFoundException_WhenExerciseDoesNotExist()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntity(sessionId, userId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            var act = () => _sut.DeleteWorkoutExerciseAsync(sessionId, Guid.NewGuid(), userId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _workoutSessionRepository.DidNotReceive().DeleteWorkoutExerciseAsync(Arg.Any<WorkoutExercise>());
        }

        #endregion

        #region AddExerciseSetAsync

        [Fact]
        public async Task AddExerciseSetAsync_ShouldAddSet_WhenExerciseExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntityWithExerciseId(sessionId, userId, exerciseId);
            var request = new CreateExerciseSetRequest { SetNumber = 2, Reps = 12, Weight = 50 };

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            await _sut.AddExerciseSetAsync(sessionId, exerciseId, request, userId);

            // Assert
            await _workoutSessionRepository.Received(1).AddExerciseSetAsync(Arg.Is<WorkoutExerciseSet>(s =>
                s.WorkoutExerciseId == exerciseId &&
                s.Reps == 12 &&
                s.Weight == 50
            ));
        }

        [Fact]
        public async Task AddExerciseSetAsync_ShouldThrowNotFoundException_WhenExerciseDoesNotExist()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntity(sessionId, userId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            var act = () => _sut.AddExerciseSetAsync(sessionId, Guid.NewGuid(), new CreateExerciseSetRequest(), userId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _workoutSessionRepository.DidNotReceive().AddExerciseSetAsync(Arg.Any<WorkoutExerciseSet>());
        }

        #endregion

        #region UpdateExerciseSetAsync

        [Fact]
        public async Task UpdateExerciseSetAsync_ShouldUpdateSet_WhenSetExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var setId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntityWithIds(sessionId, userId, exerciseId, setId);
            var request = new UpdateExerciseSetRequest { SetNumber = 1, Reps = 15, Weight = 60 };

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            await _sut.UpdateExerciseSetAsync(sessionId, exerciseId, setId, request, userId);

            // Assert
            var set = session.WorkoutExercises.First().WorkoutExerciseSets.First(s => s.Id == setId);
            Assert.Equal(15, set.Reps);
            Assert.Equal(60, set.Weight);
            await _workoutSessionRepository.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateExerciseSetAsync_ShouldThrowNotFoundException_WhenSetDoesNotExist()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntityWithExerciseId(sessionId, userId, exerciseId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            var act = () => _sut.UpdateExerciseSetAsync(sessionId, exerciseId, Guid.NewGuid(), new UpdateExerciseSetRequest(), userId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _workoutSessionRepository.DidNotReceive().SaveChangesAsync();
        }

        #endregion

        #region DeleteExerciseSetAsync

        [Fact]
        public async Task DeleteExerciseSetAsync_ShouldDeleteSet_WhenSetExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var setId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntityWithIds(sessionId, userId, exerciseId, setId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            await _sut.DeleteExerciseSetAsync(sessionId, exerciseId, setId, userId);

            // Assert
            await _workoutSessionRepository.Received(1).DeleteExerciseSetAsync(Arg.Is<WorkoutExerciseSet>(s => s.Id == setId));
        }

        [Fact]
        public async Task DeleteExerciseSetAsync_ShouldThrowNotFoundException_WhenSetDoesNotExist()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var session = CreateStandardWorkoutEntityWithExerciseId(sessionId, userId, exerciseId);

            _workoutSessionRepository
                .GetWorkoutByIdAsync(sessionId, userId, true)
                .Returns(session);

            // Act
            var act = () => _sut.DeleteExerciseSetAsync(sessionId, exerciseId, Guid.NewGuid(), userId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _workoutSessionRepository.DidNotReceive().DeleteExerciseSetAsync(Arg.Any<WorkoutExerciseSet>());
        }

        #endregion

        #region Helpers

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
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ExerciseId = Guid.NewGuid(),
                        WorkoutExerciseSets = new List<WorkoutExerciseSet>
                        {
                            new() { Id = Guid.NewGuid(), Reps = 10, Weight = 100 }
                        }
                    }
                }
            };

        private static WorkoutSession CreateStandardWorkoutEntityWithExerciseId(Guid workoutSessionId, string userId, Guid exerciseId)
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
                    new()
                    {
                        Id = exerciseId,
                        ExerciseId = Guid.NewGuid(),
                        WorkoutExerciseSets = new List<WorkoutExerciseSet>
                        {
                            new() { Id = Guid.NewGuid(), Reps = 10, Weight = 100 }
                        }
                    }
                }
            };

        private static WorkoutSession CreateStandardWorkoutEntityWithIds(Guid workoutSessionId, string userId, Guid exerciseId, Guid setId)
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
                    new()
                    {
                        Id = exerciseId,
                        ExerciseId = Guid.NewGuid(),
                        WorkoutExerciseSets = new List<WorkoutExerciseSet>
                        {
                            new() { Id = setId, Reps = 10, Weight = 100 }
                        }
                    }
                }
            };

        #endregion
    }
}
