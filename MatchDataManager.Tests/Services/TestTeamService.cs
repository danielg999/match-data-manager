using FluentValidation;
using FluentValidation.Results;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;
using MatchDataManager.Api.Repository;
using MatchDataManager.Api.Services;
using Moq;

namespace MatchDataManager.Tests.Services
{
    public class TestTeamService
    {
        private Mock<ITeamRepository> _mockTeamRepository = new Mock<ITeamRepository>();
        private Mock<IValidator<TeamCreateDto>> _mockCreateValidator = new Mock<IValidator<TeamCreateDto>>();
        private Mock<IValidator<TeamUpdateDto>> _mockUpdateValidator = new Mock<IValidator<TeamUpdateDto>>();

        [Fact]
        public void Should_GetAllTeams_When_TeamsExists()
        {
            // Arrange
            _mockTeamRepository.Setup(r => r.GetAll()).Returns(Task.FromResult(It.IsAny<IEnumerable<TeamDto>>()));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.GetAll();

            // Assert
            _mockTeamRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void Should_GetTeams_When_TeamExists()
        {
            // Arrange
            _mockTeamRepository.Setup(r => r.Get(It.IsAny<Guid>())).Returns(Task.FromResult(new TeamDto()));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.Get(It.IsAny<Guid>());

            // Assert
            _mockTeamRepository.Verify(r => r.Get(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void Should_ThrowException_DuringGetTeamById_When_TeamDoesntExist()
        {
            // Assert
            _mockTeamRepository.Setup(r => r.Get(It.IsAny<Guid>())).Throws(new NotFoundException("Team not found."));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Get(It.IsAny<Guid>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(result);
        }

        [Fact]
        public void Should_CreateTeam_When_CorrectData()
        {
            // Arrange
            var teamDto = new TeamCreateDto()
            {
                Name = "Test name",
                CoachName = "Test coach name"
            };
            _mockTeamRepository.Setup(r => r.Create(teamDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            _mockTeamRepository.Setup(r => r.CountAllTeams()).Returns(Task.FromResult(1));
            _mockTeamRepository.Setup(r => r.IsAnyExistOnCreate(teamDto.Name)).Returns(Task.FromResult(false));
            _mockCreateValidator.Setup(r => r.Validate(teamDto)).Returns(new ValidationResult());
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.Create(teamDto);

            // Assert
            _mockTeamRepository.Verify(r => r.Create(teamDto), Times.Once);
        }

        [Fact]
        public void Should_ThrowException_DuringCreateTeam_When_TeamNameIsNotUnique()
        {
            // Arrange
            var teamDto = new TeamCreateDto()
            {
                Name = "Test name",
                CoachName = "Test coach name"
            };
            _mockTeamRepository.Setup(r => r.Create(teamDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            _mockCreateValidator.Setup(r => r.Validate(teamDto)).Returns(new ValidationResult());
            _mockTeamRepository.Setup(r => r.IsAnyExistOnCreate(teamDto.Name)).Returns(Task.FromResult(true));
            _mockTeamRepository.Setup(r => r.CountAllTeams()).Returns(Task.FromResult(1000));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Create(teamDto);

            // Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Theory]
        [InlineData(0, 1, "Name")]
        [InlineData(256, 1, "Name")]
        [InlineData(1, 56, "CoachName")]
        public void Should_ThrowException_DuringCreateTeam_When_IncorrectDataTeam(int nameLength, int coachNameLength, string propertyName)
        {
            // Arrange
            var teamDto = new TeamCreateDto()
            {
                Name = new string('a', nameLength),
                CoachName = new string('a', coachNameLength)
            };
            _mockTeamRepository.Setup(r => r.Create(teamDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            var failures = new List<ValidationFailure>();
            failures.Add(new ValidationFailure(propertyName, ""));
            _mockTeamRepository.Setup(r => r.IsAnyExistOnCreate(teamDto.Name)).Returns(Task.FromResult(false));
            _mockCreateValidator.Setup(r => r.Validate(It.IsAny<TeamCreateDto>())).Returns(new ValidationResult(failures));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Create(teamDto);

            // Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public void Should_UpdateTeam_When_CorrectData()
        {
            // Arrange
            var teamDto = new TeamUpdateDto()
            {
                Name = "Test name",
                CoachName = "Test coach name"
            };
            _mockTeamRepository.Setup(r => r.Update(teamDto)).Returns(Task.CompletedTask);
            _mockUpdateValidator.Setup(r => r.Validate(It.IsAny<TeamUpdateDto>())).Returns(new ValidationResult());
            _mockTeamRepository.Setup(r => r.IsAnyExistOnCreate(teamDto.Name)).Returns(Task.FromResult(false));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.Update(teamDto);

            // Assert
            _mockTeamRepository.Verify(r => r.Update(teamDto), Times.Once);
        }

        [Fact]
        public void Should_ThrowException_DuringUpdateTeam_When_TeamNameIsNotUnique()
        {
            // Arrange
            var teamDto = new TeamUpdateDto()
            {
                Id = It.IsAny<Guid>(),
                Name = "Test name",
                CoachName = "Test coach name"
            };
            _mockTeamRepository.Setup(r => r.Update(teamDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            _mockUpdateValidator.Setup(r => r.Validate(teamDto)).Returns(new ValidationResult());
            _mockTeamRepository.Setup(r => r.IsAnyExistOnCreate(teamDto.Name)).Returns(Task.FromResult(true));
            _mockTeamRepository.Setup(r => r.CountAllTeams()).Returns(Task.FromResult(1000));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Update(teamDto);

            // Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Theory]
        [InlineData(0, 1, "Name")]
        [InlineData(256, 1, "Name")]
        [InlineData(1, 56, "CoachName")]
        public void Should_ThrowException_DuringUpdateTeam_When_IncorrectDataTeam(int nameLength, int coachNameLength, string propertyName)
        {
            // Arrange
            var teamDto = new TeamUpdateDto()
            {
                Name = new string('a', nameLength),
                CoachName = new string('a', coachNameLength)
            };
            _mockTeamRepository.Setup(r => r.Update(teamDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            var failures = new List<ValidationFailure>();
            failures.Add(new ValidationFailure(propertyName, ""));
            _mockTeamRepository.Setup(r => r.IsAnyExistOnCreate(teamDto.Name)).Returns(Task.FromResult(false));
            _mockUpdateValidator.Setup(r => r.Validate(It.IsAny<TeamUpdateDto>())).Returns(new ValidationResult(failures));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Update(teamDto);

            // Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public void Should_DeleteTeam_When_CorrectId()
        {
            // Assert
            _mockTeamRepository.Setup(r => r.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.Delete(It.IsAny<Guid>());

            // Assert
            _mockTeamRepository.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void Should_ThrowException_DuringTeamDelete_When_IdDoesntExist()
        {
            // Assert
            _mockTeamRepository.Setup(r => r.Delete(It.IsAny<Guid>())).Throws(new NotFoundException("Team not found."));
            var sut = new TeamService(_mockTeamRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Delete(It.IsAny<Guid>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(result);
        }
    }
}