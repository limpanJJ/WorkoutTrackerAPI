using NSubstitute;
using WorkoutTrackerAPI.Dtos.Exercises.Requests;
using WorkoutTrackerAPI.Exceptions;
using WorkoutTrackerAPI.Models;
using WorkoutTrackerAPI.Repositories;
using WorkoutTrackerAPI.Services;

namespace WorkoutTrackerAPI.Tests
{
    public class ExerciseServiceTests
    {
        private readonly ExerciseService _sut;
        private readonly IExerciseRepository _exerciseRepository = Substitute.For<IExerciseRepository>();

        public ExerciseServiceTests()
        {
            _sut = new ExerciseService(_exerciseRepository);
        }

        [Fact]
        public async Task GetExerciseByIdAsync_ShouldReturnExercise_WhenExerciseExists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var exerciseId = Guid.NewGuid();
            var exercise = CreateStandardBenchPressExercise(exerciseId, userId);

            _exerciseRepository
                .GetExerciseByIdAsync(exerciseId, userId, false)
                .Returns(exercise);

            // Act
            var result = await _sut.GetExerciseByIdAsync(exerciseId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(exerciseId, result.Id);
            Assert.Equal("Bench Press", result.Name);
            Assert.Equal("Strength", result.CategoryName);
            Assert.Equal("Chest", result.MuscleGroupName);

        }

        [Fact]
        public async Task GetExerciseByIdAsync_ShouldThrowNotFoundException_WhenExerciseDoesNotExist()
        {
            // Arrange
            _exerciseRepository
                .GetExerciseByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), false)
                .Returns((Exercise?)null);

            // Act
            var act = () => _sut.GetExerciseByIdAsync(Guid.NewGuid(), Guid.NewGuid().ToString());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }
        [Fact]
        public async Task CreateExerciseAsync_ShouldReturnExercise_WhenExerciseDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new CreateExerciseRequest
            {
                Name = "Bench Press",
                CategoryId = 1,
                MuscleGroupId = 1
            };
            var exercise = CreateStandardBenchPressExercise(Guid.NewGuid(), userId);

            _exerciseRepository
                .ExistsAsync(request.Name, userId)
                .Returns(false);

            _exerciseRepository
                .CreateExerciseAsync(Arg.Any<Exercise>())
                .Returns(exercise);

            // Act
            var result = await _sut.CreateExerciseAsync(request, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Bench Press", result.Name);
            Assert.Equal("Strength", result.CategoryName);
            Assert.Equal("Chest", result.MuscleGroupName);
        }

        [Fact]
        public async Task CreateExerciseAsync_ShouldThrowConflictException_WhenExerciseAlreadyExists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var request = new CreateExerciseRequest
            {
                Name = "Bench Press",
                CategoryId = 1,
                MuscleGroupId = 1
            };

            _exerciseRepository
                .ExistsAsync(request.Name, userId)
                .Returns(true);

            // Act
            var act = () => _sut.CreateExerciseAsync(request, userId);

            // Assert
            await Assert.ThrowsAsync<ConflictException>(act);
            await _exerciseRepository.DidNotReceive().CreateExerciseAsync(Arg.Any<Exercise>());
        }

        [Fact]
        public async Task UpdateExerciseAsync_ShouldUpdateExercise_WhenExerciseExists()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var request = new UpdateExerciseRequest
            {
                Name = "Bench Press",
                CategoryId = 1,
                MuscleGroupId = 1,
            };
            var exercise = new Exercise
            {
                Id = exerciseId,
                Name = "Chest Press",
                CategoryId = 2,
                Category = new ExerciseCategory { Id = 2, Name = "Cardio" },
                MuscleGroupId = 2,
                MuscleGroup = new MuscleGroup { Id = 2, Name = "Shoulders" },
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _exerciseRepository.GetExerciseByIdAsync(exerciseId, userId, true)
                .Returns(exercise);

            // Act
            await _sut.UpdateExerciseAsync(exerciseId, request, userId);

            // Assert
            Assert.Equal("Bench Press", exercise.Name);
            Assert.Equal(1, exercise.CategoryId);
            Assert.Equal(1, exercise.MuscleGroupId);
            await _exerciseRepository.Received(1).SaveChangesAsync();

        }
        [Fact]
        public async Task UpdateExerciseAsync_ShouldThrowNotFoundException_WhenExerciseDoesNotExist()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var request = new UpdateExerciseRequest
            {
                Name = "Bench Press",
                CategoryId = 1,
                MuscleGroupId = 1,
            };

            _exerciseRepository
                .GetExerciseByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), true)
                .Returns((Exercise?)null);
            // Act
            var act = () => _sut.UpdateExerciseAsync(exerciseId, request, userId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _exerciseRepository.DidNotReceive().SaveChangesAsync();

        }

        [Fact]
        public async Task UpdateExerciseAsync_ShouldThrowUnauthorizedException_WhenUserDoesNotOwnExercise()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var request = new UpdateExerciseRequest
            {
                Name = "Bench Press",
                CategoryId = 1,
                MuscleGroupId = 1,
            };

            var exercise = new Exercise
            {
                Id = exerciseId,
                Name = "Chest Press",
                CategoryId = 2,
                Category = new ExerciseCategory { Id = 2, Name = "Cardio" },
                MuscleGroupId = 2,
                MuscleGroup = new MuscleGroup { Id = 2, Name = "Shoulders" },
                UserId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow
            };

            _exerciseRepository.GetExerciseByIdAsync(exerciseId, userId, true)
                .Returns(exercise);

            // Act
            var act = () => _sut.UpdateExerciseAsync(exerciseId, request, userId);

            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(act);
            await _exerciseRepository.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteExerciseAsync_ShouldDeleteExercise_WhenExerciseExists()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var exercise = CreateStandardBenchPressExercise(exerciseId, userId);

            _exerciseRepository.GetExerciseByIdAsync(exerciseId, userId, true)
                .Returns(exercise);
            // Act
            await _sut.DeleteExerciseAsync(exerciseId, userId);

            // Assert
            await _exerciseRepository.Received(1).DeleteExerciseAsync(exercise);
        }

        [Fact]
        public async Task DeleteExerciseAsync_ShouldThrowNotFoundException_WhenExerciseDoesNotExist()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            _exerciseRepository
                .GetExerciseByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), true)
                .Returns((Exercise?)null);
            // Act
            var act = () => _sut.DeleteExerciseAsync(exerciseId, userId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
            await _exerciseRepository.DidNotReceive().DeleteExerciseAsync(Arg.Any<Exercise>());
        }
        [Fact]
        public async Task DeleteExerciseAsync_ShouldThrowUnauthorizedException_WhenUserDoesNotOwnExercise()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var exercise = CreateStandardBenchPressExercise(exerciseId, Guid.NewGuid().ToString());
            _exerciseRepository.GetExerciseByIdAsync(exerciseId, userId, true)
                .Returns(exercise);
            // Act
            var act = () => _sut.DeleteExerciseAsync(exerciseId, userId);
            // Assert
            await Assert.ThrowsAsync<UnauthorizedException>(act);
            await _exerciseRepository.DidNotReceive().DeleteExerciseAsync(Arg.Any<Exercise>());
        }

        private static Exercise CreateStandardBenchPressExercise(Guid exerciseId, string userId)
            => new()
            {
                Id = exerciseId,
                Name = "Bench Press",
                CategoryId = 1,
                Category = new ExerciseCategory { Id = 1, Name = "Strength" },
                MuscleGroupId = 1,
                MuscleGroup = new MuscleGroup { Id = 1, Name = "Chest" },
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

    }
}
