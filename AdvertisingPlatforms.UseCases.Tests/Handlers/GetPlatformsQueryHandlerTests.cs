using AdvertisingPlatforms.Entities.Models;
using AdvertisingPlatforms.Infrastructure.Interfaces.Services;
using AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Queries.GetPlatforms;
using Moq;

namespace AdvertisingPlatforms.UseCases.Tests.Handlers
{
    public class GetPlatformsQueryHandlerTests
    {
        private readonly Mock<ILocationDataRepository> _repoMock = new();
        private readonly GetPlatformsQueryHandler _handler;

        private readonly List<LocationData> _testData = new()
        {
            new LocationData
            {
                Title = "Platform1",
                Locations = new List<LocationSegments>
                {
                    new() { Segments = ["ru"] },
                }
            },
            new LocationData
            {
                Title = "Platform2",
                Locations = new List<LocationSegments> 
                {
                    new() { Segments = ["ru", "msk"] }, 
                    new() { Segments = ["ru", "perm"] },
                }
            },
            new LocationData
            {
                Title = "Platform3",
                Locations = new List<LocationSegments>
                {
                    new() { Segments = ["ru", "svrd", "revda"] },
                }
            },
        };

        public GetPlatformsQueryHandlerTests()
        {
            _repoMock.Setup(x => x.LocationData).Returns(_testData);

            _handler = new GetPlatformsQueryHandler(_repoMock.Object);
        }

        [Theory]
        [InlineData("/ru", new[] { "Platform1" })]
        [InlineData("/ru/msk", new[] { "Platform1", "Platform2" })]
        [InlineData("/ru/svrd/revda", new[] { "Platform1", "Platform3" })]
        [InlineData("/ru/test", new[] { "Platform1" })]
        public async Task Handle_ValidLocation_ReturnsCorrectPlatforms(string location, string[] expectedPlatforms)
        {
            // Arrange
            var query = new GetPlatformsQuery()
            {
                Location = location,
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(Status.Success, result.Status);

            Assert.Equal(expectedPlatforms, result.Data.Platforms);
        }

        [Theory]
        [InlineData("")]
        public async Task Handle_EmptyLocation_ReturnsBadData(string location)
        {
            // Arrange
            var query = new GetPlatformsQuery()
            {
                Location = location,
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(Status.BadData, result.Status);
        }

        [Fact]
        public async Task Handle_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _repoMock.Setup(x => x.LocationData).Returns([]);

            var query = new GetPlatformsQuery()
            {
                Location = "/ru",
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result.Data.Platforms);
        }
    }
}
