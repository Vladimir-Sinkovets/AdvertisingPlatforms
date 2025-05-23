using AdvertisingPlatforms.Infrastructure.Interfaces.Services;
using AdvertisingPlatforms.UseCases;
using AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Commands.SetAdvertisingPlatformsData;
using Moq;
using System.Text;

namespace AdvertisingPlatforms.Tests.UnitTests.Commands
{
    public class SetAdvertisingPlatformsDataCommandHandlerTests
    {
        private readonly Mock<ILocationDataRepository> _repoMock = new();
        private readonly SetAdvertisingPlatformsDataCommandHandler _handler;

        public SetAdvertisingPlatformsDataCommandHandlerTests()
        {
            _handler = new SetAdvertisingPlatformsDataCommandHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ValidData_SavesCorrectLocations()
        {
            // Arrange
            _repoMock.SetupProperty(x => x.LocationData);

            var data = new StringBuilder()
                .AppendLine("Platform1:/ru/msk,/ru/perm")
                .AppendLine("Platform2:/en/london")
                .ToString();

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));

            var command = new SetAdvertisingPlatformsDataCommand { Stream = stream };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Status.Success, result.Status);

            Assert.NotNull(_repoMock.Object.LocationData);

            Assert.Equal(2, _repoMock.Object.LocationData.Count());

            var platform1 = _repoMock.Object.LocationData.ElementAt(0);

            Assert.Equal("Platform1", platform1.Title);
            Assert.Collection(platform1.Locations,
                x => Assert.Equal(["ru", "msk"], x.Segments),
                x => Assert.Equal(["ru", "perm"], x.Segments)
            );

            var platform2 = _repoMock.Object.LocationData.ElementAt(1);

            Assert.Equal("Platform2", platform2.Title);
            Assert.Collection(platform2.Locations,
                x => Assert.Equal(["en", "london"], x.Segments));
        }

        [Theory]
        [InlineData("Platform1")]
        [InlineData(":value")]
        public async Task Handle_InvalidLineFormat_ReturnsError(string invalidLine)
        {
            // Arrange
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidLine));

            var command = new SetAdvertisingPlatformsDataCommand()
            {
                Stream = stream
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Status.BadData, result.Status);
        }

        [Fact]
        public async Task Handle_ReadError_ReturnsServerError()
        {
            // Arrange
            var stream = new Mock<Stream>();
            stream.Setup(s => s.CanRead).Returns(true);
            stream.Setup(s => s.ReadAsync(It.IsAny<Memory<byte>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new IOException());

            var command = new SetAdvertisingPlatformsDataCommand()
            {
                Stream = stream.Object,
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Status.BadData, result.Status);
        }
    }
}