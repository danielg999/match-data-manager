using MatchDataManager.Api.Models;
using MatchDataManager.Api.Models.Validations;
using MatchDataManager.Api.Repository;
using Moq;

namespace MatchDataManager.Tests.Validators
{
    public class TestTeamCreateDtoValidator
    {
        [Theory]
        [InlineData(256, false)]
        [InlineData(255, true)]
        [InlineData(20, true)]
        [InlineData(0, false)]
        public void Should_GiveIsNotValid_DuringValidation_WhenTeamNameTooLong(int nameLength, bool isNameLengthRight)
        {
            // Arrange
            var mockTeamRepository = new Mock<ITeamRepository>();
            mockTeamRepository.Setup(x => x.IsAnyExistOnCreate(It.IsAny<string>())).ReturnsAsync(false);
            var createTeamDtoValidator = new TeamCreateDtoValidator(mockTeamRepository.Object);
            var teamDto = new TeamCreateDto()
            {
                CoachName = "Coach name test",
                Name = new string('a', nameLength)
            };

            // Act
            var isValid = createTeamDtoValidator.Validate(teamDto).IsValid;

            // Assert
            Assert.Equal(isNameLengthRight, isValid);
        }

        [Theory]
        [InlineData(56, false)]
        [InlineData(55, true)]
        [InlineData(20, true)]
        [InlineData(0, true)]
        public void Should_GiveIsNotValid_DuringValidation_WhenTeamCoachNameTooLong(int nameLength, bool isNameLengthRight)
        {
            // Arrange
            var mockTeamRepository = new Mock<ITeamRepository>();
            mockTeamRepository.Setup(x => x.IsAnyExistOnCreate(It.IsAny<string>())).ReturnsAsync(false);
            var createTeamDtoValidator = new TeamCreateDtoValidator(mockTeamRepository.Object);
            var teamDto = new TeamCreateDto()
            {
                CoachName = new string('a', nameLength),
                Name = "Name test"
            };

            // Act
            var isValid = createTeamDtoValidator.Validate(teamDto).IsValid;

            // Assert
            Assert.Equal(isNameLengthRight, isValid);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void Should_GiveIsNotValid_DuringValidation_WhenTeamNameExists(bool isAnyNameExists, bool exceptedResult)
        {
            // Arrange
            var mockTeamRepository = new Mock<ITeamRepository>();
            mockTeamRepository.Setup(x => x.IsAnyExistOnCreate(It.IsAny<string>())).ReturnsAsync(isAnyNameExists);
            var createTeamDtoValidator = new TeamCreateDtoValidator(mockTeamRepository.Object);
            var teamDto = new TeamCreateDto()
            {
                CoachName = "Coach name test",
                Name = "Name test"
            };

            // Act
            var isValid = createTeamDtoValidator.Validate(teamDto).IsValid;

            // Assert
            Assert.Equal(exceptedResult, isValid);
        }
    }
}