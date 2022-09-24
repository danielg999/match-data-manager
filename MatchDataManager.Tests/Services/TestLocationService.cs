using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;
using MatchDataManager.Api.Repository;
using MatchDataManager.Api.Services;
using Moq;

namespace MatchDataManager.Tests.Services
{
    public class TestLocationService
    {
        private Mock<ILocationRepository> _mockLocationRepository = new Mock<ILocationRepository>();
        private Mock<IValidator<LocationCreateDto>> _mockCreateValidator = new Mock<IValidator<LocationCreateDto>>();
        private Mock<IValidator<LocationUpdateDto>> _mockUpdateValidator = new Mock<IValidator<LocationUpdateDto>>();

        [Fact]
        public void Should_GetAllLocations_When_LocationsExists()
        {
            // Arrange
            _mockLocationRepository.Setup(r => r.GetAll()).Returns(Task.FromResult(It.IsAny<IEnumerable<LocationDto>>()));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.GetAll();

            // Assert
            _mockLocationRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void Should_GetLocations_When_LocationExists()
        {
            // Arrange
            _mockLocationRepository.Setup(r => r.Get(It.IsAny<Guid>())).Returns(Task.FromResult(new LocationDto()));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.Get(It.IsAny<Guid>());

            // Assert
            _mockLocationRepository.Verify(r => r.Get(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void Should_ThrowException_DuringGetLocationById_When_LocationDoesntExist()
        {
            // Assert
            _mockLocationRepository.Setup(r => r.Get(It.IsAny<Guid>())).Throws(new NotFoundException("Location not found."));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Get(It.IsAny<Guid>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(result);
        }

        [Fact]
        public void Should_CreateLocation_When_CorrectData()
        {
            // Arrange
            var locationDto = new LocationCreateDto()
            {
                Name = "Test name",
                City = "Test city"
            };
            _mockLocationRepository.Setup(r => r.Create(locationDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            _mockLocationRepository.Setup(r => r.CountAllLocations()).Returns(Task.FromResult(1));
            _mockLocationRepository.Setup(r => r.IsAnyExistOnCreate(locationDto.Name)).Returns(Task.FromResult(false));
            _mockCreateValidator.Setup(r => r.Validate(locationDto)).Returns(new ValidationResult());
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.Create(locationDto);

            // Assert
            _mockLocationRepository.Verify(r => r.Create(locationDto), Times.Once);
        }

        [Fact]
        public void Should_ThrowException_DuringCreateLocation_When_LocationNameIsNotUnique()
        {
            // Arrange
            var locationDto = new LocationCreateDto()
            {
                Name = "Test name",
                City = "Test city"
            };
            _mockLocationRepository.Setup(r => r.Create(locationDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            _mockCreateValidator.Setup(r => r.Validate(locationDto)).Returns(new ValidationResult());
            _mockLocationRepository.Setup(r => r.IsAnyExistOnCreate(locationDto.Name)).Returns(Task.FromResult(true));
            _mockLocationRepository.Setup(r => r.CountAllLocations()).Returns(Task.FromResult(1000));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Create(locationDto);

            // Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Theory]
        [InlineData(0, 1, "Name")]
        [InlineData(256, 1, "Name")]
        [InlineData(1, 56, "City")]
        [InlineData(1, 0, "City")]
        public void Should_ThrowException_DuringCreateLocation_When_IncorrectDataLocation(int nameLength, int cityLength, string propertyName)
        {
            // Arrange
            var locationDto = new LocationCreateDto()
            {
                Name = new string('a', nameLength),
                City = new string('a', cityLength)
            };
            _mockLocationRepository.Setup(r => r.Create(locationDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            var failures = new List<ValidationFailure>();
            failures.Add(new ValidationFailure(propertyName, ""));
            _mockLocationRepository.Setup(r => r.IsAnyExistOnCreate(locationDto.Name)).Returns(Task.FromResult(false));
            _mockCreateValidator.Setup(r => r.Validate(It.IsAny<LocationCreateDto>())).Returns(new ValidationResult(failures));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Create(locationDto);

            // Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public void Should_UpdateLocation_When_CorrectData()
        {
            // Arrange
            var locationDto = new LocationUpdateDto()
            {
                Name = "Test name",
                City = "Test city"
            };
            _mockLocationRepository.Setup(r => r.Update(locationDto)).Returns(Task.CompletedTask);
            _mockUpdateValidator.Setup(r => r.Validate(It.IsAny<LocationUpdateDto>())).Returns(new ValidationResult());
            _mockLocationRepository.Setup(r => r.IsAnyExistOnCreate(locationDto.Name)).Returns(Task.FromResult(false));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.Update(locationDto);

            // Assert
            _mockLocationRepository.Verify(r => r.Update(locationDto), Times.Once);
        }

        [Fact]
        public void Should_ThrowException_DuringUpdateLocation_When_LocationNameIsNotUnique()
        {
            // Arrange
            var locationDto = new LocationUpdateDto()
            {
                Id = It.IsAny<Guid>(),
                Name = "Test name",
                City = "Test city"
            };
            _mockLocationRepository.Setup(r => r.Update(locationDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            _mockUpdateValidator.Setup(r => r.Validate(locationDto)).Returns(new ValidationResult());
            _mockLocationRepository.Setup(r => r.IsAnyExistOnCreate(locationDto.Name)).Returns(Task.FromResult(true));
            _mockLocationRepository.Setup(r => r.CountAllLocations()).Returns(Task.FromResult(1000));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Update(locationDto);

            // Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Theory]
        [InlineData(0, 1, "Name")]
        [InlineData(256, 1, "Name")]
        [InlineData(1, 56, "City")]
        [InlineData(1, 0, "City")]
        public void Should_ThrowException_DuringUpdateLocation_When_IncorrectDataLocation(int nameLength, int cityLength, string propertyName)
        {
            // Arrange
            var locationDto = new LocationUpdateDto()
            {
                Name = new string('a', nameLength),
                City = new string('a', cityLength)
            };
            _mockLocationRepository.Setup(r => r.Update(locationDto)).Returns(Task.FromResult(It.IsAny<Guid>()));
            var failures = new List<ValidationFailure>();
            failures.Add(new ValidationFailure(propertyName, ""));
            _mockLocationRepository.Setup(r => r.IsAnyExistOnCreate(locationDto.Name)).Returns(Task.FromResult(false));
            _mockUpdateValidator.Setup(r => r.Validate(It.IsAny<LocationUpdateDto>())).Returns(new ValidationResult(failures));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Update(locationDto);

            // Assert
            Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public void Should_DeleteLocation_When_CorrectId()
        {
            // Assert
            _mockLocationRepository.Setup(r => r.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = sut.Delete(It.IsAny<Guid>());

            // Assert
            _mockLocationRepository.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void Should_ThrowException_DuringLocationDelete_When_IdDoesntExist()
        {
            // Assert
            _mockLocationRepository.Setup(r => r.Delete(It.IsAny<Guid>())).Throws(new NotFoundException("Location not found."));
            var sut = new LocationService(_mockLocationRepository.Object, _mockCreateValidator.Object,
                _mockUpdateValidator.Object);

            // Act
            var result = async () => await sut.Delete(It.IsAny<Guid>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(result);
        }
    }
}
