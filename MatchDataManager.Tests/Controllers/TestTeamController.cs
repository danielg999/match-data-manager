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
    public class TestTeamController
    {
        [Fact]
        public async Task GetAll_OnSuccess_Returns200()
        {
            // Arrange
            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.GetAll())
                .ReturnsAsync(TeamFixture.GetAll());
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetAll();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Get_OnSuccess_Returns200()
        {
            // Arrange
            var teamId = It.IsAny<Guid>();
            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.Get(teamId))
                .ReturnsAsync(new TeamDto());
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var result = (OkObjectResult)await sut.Get(teamId);

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Get_OnNoTeamFound_Returns404()
        {
            // Arrange
            var teamId = It.IsAny<Guid>();
            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.Get(teamId))
                .Throws(new NotFoundException(It.IsAny<string>()));
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var action = async () => await sut.Get(teamId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);
        }

        [Fact]
        public async Task Create_OnSuccess_Returns201()
        {
            // Arrange
            var teamDto = new TeamCreateDto()
            {
                Name = "Name test",
                CoachName = "Coach name test"
            };
            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.Create(teamDto))
                .ReturnsAsync(Guid.NewGuid);
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var result = (ObjectResult)await sut.Create(teamDto);

            // Assert
            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task Create_WithInvalidModel_ReturnsBadRequestException()
        {
            // Arrange
            var teamDto = new TeamCreateDto()
            {
                CoachName = "Coach name test" // Without name
            };

            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.Create(teamDto))
                .Throws(new BadRequestException(It.IsAny<string>()));
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var action = async () => await sut.Create(teamDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        [Fact]
        public async Task Create_OnSuccess_InvokesTeamServiceExactlyOnce()
        {
            // Arrange
            var teamDto = new TeamCreateDto()
            {
                Name = "Name test",
                CoachName = "Coach name test"
            };
            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.Create(teamDto))
                .ReturnsAsync(Guid.NewGuid);
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var result = await sut.Create(teamDto);

            // Assert
            mockTeamService.Verify(x => x.Create(teamDto), Times.Once);
        }

        [Fact]
        public async Task Update_OnSuccess_Returns200()
        {
            // Arrange
            var teamDto = new TeamUpdateDto()
            {
                Id = Guid.NewGuid(),
                Name = "Name test",
                CoachName = "Coach name test"
            };
            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.Update(teamDto));
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var result = (ObjectResult)await sut.Update(teamDto);

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Update_OnSuccess_InvokesTeamServiceExactlyOnce()
        {
            // Arrange
            var teamDto = new TeamUpdateDto()
            {
                Id = Guid.NewGuid(),
                Name = "Name test",
                CoachName = "Coach name test"
            };
            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.Update(teamDto));
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var result = await sut.Update(teamDto);

            // Assert
            mockTeamService.Verify(x => x.Update(teamDto), Times.Once);
        }

        [Fact]
        public async Task Update_WithInvalidModel_ReturnsBadRequestException()
        {
            // Arrange
            var teamDto = new TeamUpdateDto()
            {
                Id = Guid.NewGuid(),
                CoachName = "Coach name test" // Without name
            };

            var mockTeamService = new Mock<ITeamService>();
            mockTeamService
                .Setup(x => x.Update(teamDto))
                .Throws(new BadRequestException(It.IsAny<string>()));
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var action = async () => await sut.Update(teamDto);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(action);
        }

        [Fact]
        public async Task Delete_OnSuccess_Returns204()
        {
            // Arrange
            var teamId = Guid.NewGuid();
            var mockTeamService = new Mock<ITeamService>();
            mockTeamService.Setup(x => x.Delete(teamId));
            var sut = new TeamController(mockTeamService.Object);

            // Act
            var result = (NoContentResult)await sut.Delete(teamId);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}