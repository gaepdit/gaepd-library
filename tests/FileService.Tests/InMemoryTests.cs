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
        var fileName = $"fileName_{Guid.NewGuid().ToString()}";

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
        var fileName = $"fileName_{Guid.NewGuid().ToString()}";
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

        // In FileSystem, `GetFilesAsync` returns files in alphabetical order, so this naming makes the assertions simpler.
        // (In InMemory, files are returned in the order added, so naming doesn't matter.)
        var files = new List<(string path, string fileName)>
        {
            (searchPath, $"a_{Guid.NewGuid().ToString()}"),
            (searchPath, $"b_{Guid.NewGuid().ToString()}"),
            (ignorePath, $"c_{Guid.NewGuid().ToString()}"),
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
    public async Task GetFiles_WhenFilesExistInSubfolders_RecursivelyReturnsList()
    {
        // Arrange
        const string searchPath = "searchPath";
        const string nestedPath = "nestedPath";

        // In FileSystem, `GetFilesAsync` returns files in alphabetical order, so this naming makes the assertions simpler.
        // (In InMemory, files are returned in the order added, so naming doesn't matter.)
        var files = new List<(string path, string fileName)>
        {
            (searchPath, $"a_{Guid.NewGuid().ToString()}"),
            (searchPath, $"b_{Guid.NewGuid().ToString()}"),
            (Path.Combine(searchPath, nestedPath), $"c_{Guid.NewGuid().ToString()}"),
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

        i.Should().Be(3);
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
        var fileName = $"fileName_{Guid.NewGuid().ToString()}";
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
        var fileName = $"fileName_{Guid.NewGuid().ToString()}";
        using (var msTest = new MemoryStream(_fileBytes)) await _fileService.SaveFileAsync(msTest, fileName);

        // Act
        await _fileService.DeleteFileAsync(fileName);

        // Assert
        (await _fileService.FileExistsAsync(fileName)).Should().BeFalse();
    }

    [Test]
    public async Task DeleteFile_WithPath_Succeeds()
    {
        // Arrange
        var fileName = $"fileName_{Guid.NewGuid().ToString()}";
        var path = $"path_{Guid.NewGuid().ToString()}";
        using (var msTest = new MemoryStream(_fileBytes)) await _fileService.SaveFileAsync(msTest, fileName, path);

        // Act
        await _fileService.DeleteFileAsync(Path.Combine(path, fileName));

        // Assert
        (await _fileService.FileExistsAsync(fileName, path)).Should().BeFalse();
    }
}
