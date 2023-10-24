namespace GaEpd.FileService.Implementations;

public class InMemory : IFileService
{
    private readonly Dictionary<string, byte[]> _fileContainer = new();

    public async Task SaveFileAsync(Stream stream, string fileName, string path = "",
        CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        await using var ms = new MemoryStream(Convert.ToInt32(stream.Length));
        await stream.CopyToAsync(ms, token);
        _fileContainer.Add(Path.Combine(path, fileName), ms.ToArray());
    }

    public Task<bool> FileExistsAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        return Task.FromResult(_fileContainer.ContainsKey(Path.Combine(path, fileName)));
    }

    public Task<Stream> GetFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var filePath = Path.Combine(path, fileName);
        byte[] fileContents;

        try
        {
            fileContents = _fileContainer[filePath];
        }
        catch (KeyNotFoundException e)
        {
            throw new FileNotFoundException(filePath, e);
        }

        return Task.FromResult<Stream>(new MemoryStream(fileContents));
    }

    public Task<IFileService.TryGetResponse> TryGetFileAsync(string fileName, string path = "",
        CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var filePath = Path.Combine(path, fileName);
        return Task.FromResult(
            _fileContainer.TryGetValue(filePath, out var value)
                ? new IFileService.TryGetResponse(true, new MemoryStream(value))
                : new IFileService.TryGetResponse(false, Stream.Null));
    }

    public Task DeleteFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        _fileContainer.Remove(Path.Combine(path, fileName));
        return Task.CompletedTask;
    }
}
