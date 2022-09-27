using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchDataManager.Api.Models;
using MatchDataManager.Api.Models.Validations;
using MatchDataManager.Api.Repository;
using Moq;

namespace MatchDataManager.Tests.Validators
{
    public class TestTeamUpdateDtoValidator
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
            mockTeamRepository.Setup(x => x.IsAnyExistOnUpdate(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var updateTeamDtoValidator = new TeamUpdateDtoValidator(mockTeamRepository.Object);
            var teamDto = new TeamUpdateDto()
            {
                CoachName = "Coach name test",
                Name = new string('a', nameLength)
            };

            // Act
            var isValid = updateTeamDtoValidator.Validate(teamDto).IsValid;

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
            mockTeamRepository.Setup(x => x.IsAnyExistOnUpdate(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var updateTeamDtoValidator = new TeamUpdateDtoValidator(mockTeamRepository.Object);
            var teamDto = new TeamUpdateDto()
            {
                CoachName = new string('a', nameLength),
                Name = "Name test"
            };

            // Act
            var isValid = updateTeamDtoValidator.Validate(teamDto).IsValid;

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
            mockTeamRepository.Setup(x => x.IsAnyExistOnUpdate(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(isAnyNameExists);
            var updateTeamDtoValidator = new TeamUpdateDtoValidator(mockTeamRepository.Object);
            var teamDto = new TeamUpdateDto()
            {
                CoachName = "Coach name test",
                Name = "Name test"
            };

            // Act
            var isValid = updateTeamDtoValidator.Validate(teamDto).IsValid;

            // Assert
            Assert.Equal(exceptedResult, isValid);
        }
    }
}
