using FluentAssertions;
using MatchDataManager.Api.Controllers;
using MatchDataManager.Api.Exceptions;
using MatchDataManager.Api.Models;
using MatchDataManager.Api.Services;
using MatchDataManager.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MatchDataManager.Tests.Controllers
{
    public class TestLocationController
    {
        [Fact]
        public async Task GetAll_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.GetAll())
                .ReturnsAsync(LocationFixture.GetTestAll());
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetAll();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAll_OnSuccess_InvokesLocationServiceExactlyOnce()
        {
            // Arrange
            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.GetAll())
                .ReturnsAsync(LocationFixture.GetTestAll());
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = await sut.GetAll();

            // Assert
            mockLocationService.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_OnSuccess_ReturnsListOfLocations()
        {
            // Arrange
            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.GetAll())
                .ReturnsAsync(LocationFixture.GetTestAll());
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = await sut.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeOfType<List<LocationDto>>();
        }

        [Fact]
        public async Task Get_OnSuccess_Returns200()
        {
            // Arrange
            var locationId = It.IsAny<Guid>();
            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.Get(locationId))
                .ReturnsAsync(new LocationDto());
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = (OkObjectResult)await sut.Get(locationId);

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Get_OnNoLocationFound_Returns404()
        {
            // Arrange
            var locationId = It.IsAny<Guid>();
            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.Get(locationId))
                .Throws(new NotFoundException(It.IsAny<string>()));
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var action = async () => await sut.Get(locationId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async Task Get_OnSuccess_InvokesLocationServiceExactlyOnce()
        {
            // Arrange
            var locationId = It.IsAny<Guid>();
            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.Get(locationId))
                .ReturnsAsync(new LocationDto());
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = await sut.Get(locationId);

            // Assert
            mockLocationService.Verify(x => x.Get(locationId), Times.Once);
        }

        [Fact]
        public async Task Create_OnSuccess_Returns201()
        {
            // Arrange
            var locationDto = new LocationCreateDto()
            {
                City = "Poznan",
                Name = "Name test 1"
            };

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.Create(locationDto))
                .ReturnsAsync(Guid.NewGuid);
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = (ObjectResult)await sut.Create(locationDto);

            // Assert
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task Create_WithInvalidModel_ReturnsBadRequestException()
        {
            // Arrange
            var locationDto = new LocationCreateDto()
            {
                City = "Poznadn" // Without name
            };

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.Create(locationDto))
                .Throws(new BadRequestException(It.IsAny<string>()));
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var action = async () => await sut.Create(locationDto);
            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        [Fact]
        public async Task Create_OnSuccess_InvokesLocationServiceExactlyOnce()
        {
            // Arrange
            var locationDto = new LocationCreateDto()
            {
                City = "Poznan",
                Name = "Name test 1"
            };

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.Create(locationDto))
                .ReturnsAsync(Guid.NewGuid);
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = await sut.Create(locationDto);

            // Assert
            mockLocationService.Verify(x => x.Create(locationDto), Times.Once());
        }

        [Fact]
        public async Task Update_OnSuccess_Returns200()
        {
            // Arrange
            var locationDto = new LocationUpdateDto()
            {
                Id = Guid.NewGuid(),
                City = "Poznan",
                Name = "Name test 1"
            };

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(x => x.Update(locationDto));
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = (ObjectResult)await sut.Update(locationDto);

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Update_WithInvalidModel_ReturnsBadRequestException()
        {
            // Arrange
            var locationDto = new LocationUpdateDto()
            {
                Id = Guid.NewGuid(),
                City = "Poznan" // Without name
            };

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.Update(locationDto))
                .Throws(new BadRequestException(It.IsAny<string>()));
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var action = async () => await sut.Update(locationDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        [Fact]
        public async Task Update_OnSuccess_InvokesLocationServiceExactlyOnce()
        {
            // Arrange
            var locationDto = new LocationUpdateDto()
            {
                Id = Guid.NewGuid(),
                City = "Poznan",
                Name = "Name test 1"
            };

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(x => x.Update(locationDto));
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = (ObjectResult)await sut.Update(locationDto);

            // Assert
            mockLocationService.Verify(x => x.Update(locationDto), Times.Once());
        }

        [Fact]
        public async Task Delete_OnSuccess_Returns204()
        {
            // Arrange
            var locationId = It.IsAny<Guid>();

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(x => x.Delete(locationId));
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = (NoContentResult)await sut.Delete(locationId);

            // Assert
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Delete_OnSuccess_InvokesLocationServiceExactlyOnce()
        {
            // Arrange
            var locationId = It.IsAny<Guid>();

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(x => x.Delete(locationId));
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var result = await sut.Delete(locationId);

            // Assert
            mockLocationService.Verify(x => x.Delete(locationId), Times.Once());
        }

        [Fact]
        public async Task Delete_OnFail_ReturnsNotFoundException()
        {
            // Arrange
            var locationId = It.IsAny<Guid>();

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(x => x.Delete(locationId))
                .ThrowsAsync(new NotFoundException(It.IsAny<string>()));
            var sut = new LocationController(mockLocationService.Object);

            // Act
            var action = async () => await sut.Delete(locationId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
        }
    }
}