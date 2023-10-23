using FileNotFoundException = GaEpd.FileService.FileNotFoundException;

namespace FileService.Tests;

public class InMemoryTests
{
    private InMemory _fileService = null!;

    [SetUp]
    public void Setup() => _fileService = new InMemory();

    [Test]
    public async Task SaveFileSucceeds_AndGet_ReturnsFile()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        byte[] fileBytes = { 0x0 };

        // Act
        using (var msTest = new MemoryStream(fileBytes))
        {
            await _fileService.SaveFileAsync(msTest, fileName);
        }

        await using (var stream = await _fileService.GetFileAsync(fileName))
        {
            // Assert
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.ToArray().Should().BeEquivalentTo(fileBytes);
        }
    }

    [Test]
    public async Task Exists_WhenFileExists_ReturnsTrue()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        byte[] fileBytes = { 0x0 };
        await _fileService.SaveFileAsync(new MemoryStream(fileBytes), fileName);

        // Act
        var result = await _fileService.FileExistsAsync(fileName);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task Exists_WhenFileDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();

        // Act
        var result = await _fileService.FileExistsAsync(fileName);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task Get_WhenFileDoesNotExist_Throws()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();

        // Act
        var func = async () => await _fileService.GetFileAsync(fileName);

        // Assert
        await func.Should().ThrowAsync<FileNotFoundException>();
    }

    [Test]
    public async Task Delete_Succeeds()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        byte[] fileBytes = { 0x0 };
        await _fileService.SaveFileAsync(new MemoryStream(fileBytes), fileName);

        // Act
        await _fileService.DeleteFileAsync(fileName);

        // Assert
        var func = async () => await _fileService.GetFileAsync(fileName);
        await func.Should().ThrowAsync<FileNotFoundException>();
    }

    [Test]
    public async Task TryGet_WhenFileExists_ReturnsTrueAndFile()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        byte[] fileBytes = { 0x0 };
        await _fileService.SaveFileAsync(new MemoryStream(fileBytes), fileName);

        // Act
        var response = await _fileService.TryGetFileAsync(fileName);

        // Assert
        using var scope = new AssertionScope();
        response.Success.Should().BeTrue();

        using var ms = new MemoryStream();
        await response.Value.CopyToAsync(ms);
        ms.ToArray().Should().BeEquivalentTo(fileBytes);
    }

    [Test]
    public async Task TryGet_WhenFileDoesNotExist_ReturnsFalse()
    {
        // Act
        var response = await _fileService.TryGetFileAsync("b");

        // Assert
        response.Success.Should().BeFalse();
    }
}
