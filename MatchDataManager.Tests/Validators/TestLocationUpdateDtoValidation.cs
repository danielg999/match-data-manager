using MatchDataManager.Api.Models;
using MatchDataManager.Api.Models.Validations;
using MatchDataManager.Api.Repository;
using Moq;

namespace MatchDataManager.Tests.Validators
{
    public class TestLocationUpdateDtoValidation
    {
        [Theory]
        [InlineData(256, false)]
        [InlineData(255, true)]
        [InlineData(20, true)]
        [InlineData(0, false)]
        public void Should_GiveIsNotValid_DuringValidation_WhenLocationNameTooLong(int nameLength, bool isNameLengthRight)
        {
            // Arrange
            var mockLocationRepository = new Mock<ILocationRepository>();
            mockLocationRepository.Setup(x => x.IsAnyExistOnUpdate(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var updateLocationDtoValidator = new LocationUpdateDtoValidator(mockLocationRepository.Object);
            var locationDto = new LocationUpdateDto()
            {
                City = "City test",
                Name = new string('a', nameLength)
            };

            // Act
            var isValid = updateLocationDtoValidator.Validate(locationDto).IsValid;

            // Assert
            Assert.Equal(isNameLengthRight, isValid);
        }

        [Theory]
        [InlineData(56, false)]
        [InlineData(55, true)]
        [InlineData(25, true)]
        [InlineData(0, false)]
        public void Should_GiveIsNotValid_DuringValidation_WhenLocationCityTooLong(int cityLength, bool isCityLengthRight)
        {
            // Arrange
            var mockLocationRepository = new Mock<ILocationRepository>();
            mockLocationRepository.Setup(x => x.IsAnyExistOnUpdate(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);
            var updateLocationDtoValidator = new LocationUpdateDtoValidator(mockLocationRepository.Object);
            var locationDto = new LocationUpdateDto()
            {
                City = new string('a', cityLength),
                Name = "Name test"
            };

            // Act
            var isValid = updateLocationDtoValidator.Validate(locationDto).IsValid;

            // Assert
            Assert.Equal(isCityLengthRight, isValid);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void Should_GiveIsNotValid_DuringValidation_WhenLocationNameExists(bool isAnyNameExists, bool exceptedResult)
        {
            // Arrange
            var mockLocationRepository = new Mock<ILocationRepository>();
            mockLocationRepository.Setup(x => x.IsAnyExistOnUpdate(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(isAnyNameExists);
            var updateLocationDtoValidator = new LocationUpdateDtoValidator(mockLocationRepository.Object);
            var locationDto = new LocationUpdateDto()
            {
                City = "City test",
                Name = "Name test"
            };

            // Act
            var isValid = updateLocationDtoValidator.Validate(locationDto).IsValid;

            // Assert
            Assert.Equal(exceptedResult, isValid);
        }
    }
}