using Resources.Services;

public class FileServiceTests
{
    private readonly string _testFilePath = "testfile.json";

    [Fact]
    public void SaveToFile_ShouldWriteContentToFile()
    {
        // Arrange
        var fileService = new FileService(_testFilePath);
        var content = "Test content";

        // Act
        var result = fileService.SaveToFile(content);

        // Assert
        Assert.True(result);
        Assert.True(File.Exists(_testFilePath));

        var actualContent = File.ReadAllText(_testFilePath).Trim();  // Trim tar bort \r\n
        Assert.Equal(content, actualContent);

        // Cleanup
        File.Delete(_testFilePath);
    }


    [Fact]
    public void GetFromFile_ShouldReturnFileContent()
    {
        // Arrange
        var fileService = new FileService(_testFilePath);
        var content = "Test content";
        File.WriteAllText(_testFilePath, content);

        // Act
        var result = fileService.GetFromFile();

        // Assert
        Assert.Equal(content, result);

        // Cleanup
        File.Delete(_testFilePath);
    }

    [Fact]
    public void GetFromFile_ShouldReturnEmptyString_WhenFileDoesNotExist()
    {
        // Arrange
        var fileService = new FileService("nonexistentfile.json");

        // Act
        var result = fileService.GetFromFile();

        // Assert
        Assert.Null(result);
    }
}