using ImageManager.Model;
using ImageManager.Services;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System;

namespace ImageManager.Tests
{
    public class ImageManagerTests
    {
        private ImageManager sut;
        private Mock<IPersistentService> persistentService;
        public ImageManagerTests()
        {
            persistentService = new Mock<IPersistentService>();
            persistentService.SetupAllProperties();
            persistentService.Setup(x => x.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ImageInfo>()))
                             .ReturnsAsync(new ImageUploadedModel());
            sut = new ImageManager(persistentService.Object);
        }
        [Fact]
        public void UoW_InitialCondition_ExpectedResult()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task ProcessUploadAsync_WithNullFilesInput_ShouldThrowExceptionAsync()
        {
            // Arrange

            // Act

            // Assert
            await Assert.ThrowsAsync<Exception>(() => sut.ProcessUploadAsync(null));
        }
    }
}
