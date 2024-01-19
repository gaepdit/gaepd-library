using FileNotFoundException = GaEpd.FileService.FileNotFoundException;

namespace FileService.Tests;

public class InMemoryTests
{
    private InMemory _fileService = null!;
    private readonly byte[] _fileBytes = [0x0];

    [SetUp]
    public void Setup() => _fileService = new InMemory();

    [Test]
    public async Task SaveFile_Succeeds_And_GetFile_ReturnsFile()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();

        // Act
        using (var msTest = new MemoryStream(_fileBytes)) await _fileService.SaveFileAsync(msTest, fileName);

        await using (var stream = await _fileService.GetFileAsync(fileName))
        {
            // Assert
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.ToArray().Should().BeEquivalentTo(_fileBytes);
        }
    }

    [Test]
    public async Task FileExists_WhenFileExists_ReturnsTrue()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        using (var msTest = new MemoryStream(_fileBytes)) await _fileService.SaveFileAsync(msTest, fileName);

        // Act
        var result = await _fileService.FileExistsAsync(fileName);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task FileExists_WhenFileDoesNotExist_ReturnsFalse()
    {
        // Act
        var result = await _fileService.FileExistsAsync("nope");

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task GetFiles_WhenFilesExist_ReturnsList()
    {
        // Arrange
        const string searchPath = "searchPath";
        const string ignorePath = "ignorePath";

        var files = new List<(string path, string fileName)>
        {
            (searchPath, Guid.NewGuid().ToString()),
            (searchPath, Guid.NewGuid().ToString()),
            (ignorePath, Guid.NewGuid().ToString()),
        };

        foreach (var tuple in files)
            using (var msTest = new MemoryStream(_fileBytes))
                await _fileService.SaveFileAsync(msTest, tuple.fileName, tuple.path);

        // Act
        // -- Only find files starting with `searchPath`.
        var results = _fileService.GetFilesAsync(searchPath);

        // Assert
        var i = 0;
        await foreach (var item in results)
        {
            item.FullName.Should().Be(Path.Combine(files[i].path, files[i].fileName));
            i++;
        }

        i.Should().Be(2);
    }

    [Test]
    public async Task GetFiles_WhenFileDoesNotExist_ReturnsEmptyList()
    {
        // Act
        var results = _fileService.GetFilesAsync();

        // Assert
        await foreach (var unused in results) Assert.Fail("`results` should be empty.");
        Assert.Pass("`results` is empty.");
    }

    [Test]
    public async Task GetFile_WhenFileDoesNotExist_Throws()
    {
        // Act
        var func = async () => await _fileService.GetFileAsync("nope");

        // Assert
        await func.Should().ThrowAsync<FileNotFoundException>();
    }

    [Test]
    public async Task TryGetFile_WhenFileExists_ReturnsTrueAndFile()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        using (var msTest = new MemoryStream(_fileBytes)) await _fileService.SaveFileAsync(msTest, fileName);

        // Act
        await using (var response = await _fileService.TryGetFileAsync(fileName))
        {
            using var scope = new AssertionScope();
            response.Success.Should().BeTrue();
            using var ms = new MemoryStream();
            await response.Value.CopyToAsync(ms);
            ms.ToArray().Should().BeEquivalentTo(_fileBytes);
        }
    }

    [Test]
    public async Task TryGetFile_WhenFileDoesNotExist_ReturnsFalse()
    {
        // Assert
        (await _fileService.TryGetFileAsync("nope")).Success.Should().BeFalse();
    }

    [Test]
    public async Task DeleteFile_Succeeds()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        using (var msTest = new MemoryStream(_fileBytes)) await _fileService.SaveFileAsync(msTest, fileName);

        // Act
        await _fileService.DeleteFileAsync(fileName);

        // Assert
        var func = async () => await _fileService.GetFileAsync(fileName);
        await func.Should().ThrowAsync<FileNotFoundException>();
    }
}
