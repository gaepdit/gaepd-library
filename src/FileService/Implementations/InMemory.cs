using System.Runtime.CompilerServices;

namespace GaEpd.FileService.Implementations;

public class InMemory : IFileService
{
    private readonly Dictionary<string, MemoryFile> _fileContainer = new();

    private sealed record MemoryFile(byte[] Content, long ContentLength, DateTimeOffset CreatedOn);

    public async Task SaveFileAsync(Stream stream, string fileName, string path = "",
        CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        await using var ms = new MemoryStream(Convert.ToInt32(stream.Length));
        await stream.CopyToAsync(ms, token);
        _fileContainer.Add(Path.Combine(path, fileName),
            new MemoryFile(ms.ToArray(), ms.Length, DateTimeOffset.UtcNow));
    }

    public Task<bool> FileExistsAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        return Task.FromResult(_fileContainer.ContainsKey(Path.Combine(path, fileName)));
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async IAsyncEnumerable<IFileService.FileDescription> GetFilesAsync(string path = "",
        [EnumeratorCancellation] CancellationToken token = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        foreach (var pair in _fileContainer.Where(pair => pair.Key.StartsWith(path)))
        {
            yield return new IFileService.FileDescription
            {
                FullName = pair.Key,
                ContentLength = pair.Value.ContentLength,
                CreatedOn = pair.Value.CreatedOn,
            };
        }
    }

    public Task<Stream> GetFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var filePath = Path.Combine(path, fileName);
        MemoryFile memoryFile;

        try
        {
            memoryFile = _fileContainer[filePath];
        }
        catch (KeyNotFoundException e)
        {
            throw new FileNotFoundException(filePath, e);
        }

        return Task.FromResult<Stream>(new MemoryStream(memoryFile.Content));
    }

    public Task<IFileService.TryGetResponse> TryGetFileAsync(string fileName, string path = "",
        CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var filePath = Path.Combine(path, fileName);
        return Task.FromResult(
            _fileContainer.TryGetValue(filePath, out var memoryFile)
                ? new IFileService.TryGetResponse(true, new MemoryStream(memoryFile.Content))
                : new IFileService.TryGetResponse(false, Stream.Null));
    }

    public Task DeleteFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        _fileContainer.Remove(Path.Combine(path, fileName));
        return Task.CompletedTask;
    }
}
