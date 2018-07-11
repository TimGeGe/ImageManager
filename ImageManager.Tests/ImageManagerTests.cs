using ImageManager.Model;
using ImageManager.Services;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace ImageManager.Tests
{
    public class ImageManagerTests
    {
        private ImageManager sut;
        private Mock<IPersistentService> persistentService;
        private List<string> files;
        private MockFileSystem fileSystem;
        public ImageManagerTests()
        {
            persistentService = new Mock<IPersistentService>();
            persistentService.SetupAllProperties();
            persistentService.Setup(x => x.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                             .ReturnsAsync(new ImageUploadedModel());

            fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
                            {
                                { @"c:\myfile.txt", new MockFileData("Testing is meh.") },
                                { @"c:\demo\jQuery.js", new MockFileData("some js") },
                                { @"c:\demo\image.jpg", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
                            });

            sut = new ImageManager(persistentService.Object, fileSystem);
            files = new List<string> { null, "file1", "file2", "file3" };
        }
        //[Fact]
        //public void UoW_InitialCondition_ExpectedResult()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}
        [Fact]
        public async Task ProcessUploadAsync_WithNullFilesInput_ShouldThrowException()
        {
            // Assert
            await Assert.ThrowsAsync<Exception>(() => sut.ProcessUploadAsync(null));
        }

        [Fact]
        public async Task ProcessUploadAsync_WithDummyFiles_ShouldNotBeenCalled()
        {
            // Act
            await sut.ProcessUploadAsync(files);
            // Assert
            persistentService.Verify(a => a.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                                    Times.Never);
        }

        [Fact]
        public async Task ProcessUploadAsync_WithFiles_ReturnModels()
        {
            // Arrange
            files = new List<string> { null, @"c:\myfile.txt", @"c:\demo\jQuery.js", @"c:\demo\image.jpg" };
            // Act
            var models = await sut.ProcessUploadAsync(files);
            // Assert
            Assert.Equal(3, models.Count);
            Assert.Equal(0, models.Count(a => a.Exception != null));
        }

        [Fact]
        public async Task ProcessUploadAsync_WithSomeFailure_ResumeTheRest()
        {
            // Arrange
            files = new List<string> { null, @"c:\myfile.txt", @"c:\demo\jQuery.js", @"c:\demo\image.jpg" };
            persistentService.Setup(x => x.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>(), @"c:\myfile.txt")).Throws(new Exception());
            // Act
            var models = await sut.ProcessUploadAsync(files);
            // Assert
            Assert.Equal(3, models.Count);
            Assert.Equal(1, models.Count(a => a.Exception != null));
        }
    }
}
