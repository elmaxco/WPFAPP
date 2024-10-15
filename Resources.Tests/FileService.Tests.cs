using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources.Services;
using System.IO;
using Xunit;

namespace Resources.Tests
{
   

    public class FileServiceTests
    {
        private readonly string _testFilePath = Path.Combine(Path.GetTempPath(), "testfile.json");

        [Fact]
        public void SaveToFile_ShouldSaveContentToJsonFile()
        {
            // Arrange
            var fileService = new FileService(_testFilePath);
            var content = "{ \"ID\": 1, \"Name\": \"TestProduct\" }";

            // Act
            var result = fileService.SaveToFile(content);

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(_testFilePath));
            var fileContent = File.ReadAllText(_testFilePath);
            Assert.Equal(content, fileContent);

            // Cleanup
            File.Delete(_testFilePath);
        }

        [Fact]
        public void GetFromFile_ShouldGetContent_WhenJsonFileIsPresent()
        {
            // Arrange
            var content = "{ \"ID\": 1, \"Name\": \"TestProduct\" }";
            File.WriteAllText(_testFilePath, content);
            var fileService = new FileService(_testFilePath);

            // Act
            var result = fileService.GetFromFile();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(content, result);

            // Cleanup
            File.Delete(_testFilePath);
        }

        [Fact]
        public void GetFromFile_ShouldReturnNull_WhenFileDoesNotExist()
        {
            // Arrange
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);

            var fileService = new FileService(_testFilePath);

            // Act
            var result = fileService.GetFromFile();

            // Assert
            Assert.Null(result);
        }
    }

}
