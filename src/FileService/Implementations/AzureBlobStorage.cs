using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace GaEpd.FileService.Implementations;

public class AzureBlobStorage : IFileService
{
    private readonly string _basePath;
    private readonly BlobContainerClient _containerClient;

    public AzureBlobStorage(string accountName, string container, string basePath)
    {
        Guard.NotNullOrWhiteSpace(accountName);
        Guard.NotNullOrWhiteSpace(container);
        _basePath = basePath;
        var serviceUrl = new Uri($"https://{accountName}.blob.core.windows.net");
        var serviceClient = new BlobServiceClient(serviceUrl, new DefaultAzureCredential());
        _containerClient = serviceClient.GetBlobContainerClient(container);
    }

    public async Task SaveFileAsync(Stream stream, string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var filePath = Path.Combine(_basePath, path, fileName);
        var blobClient = _containerClient.GetBlobClient(filePath);
        await blobClient.UploadAsync(stream, overwrite: true, token);
    }

    public async Task<bool> FileExistsAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(Path.Combine(_basePath, path, fileName));
        return (await blobClient.ExistsAsync(token)).Value;
    }

    public async Task<Stream> GetFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(Path.Combine(_basePath, path, fileName));

        if (!await blobClient.ExistsAsync(token))
        {
            throw new FileNotFoundException(Path.Combine(path, fileName));
        }

        var fileResponse = await blobClient.DownloadStreamingAsync(cancellationToken: token);
        return fileResponse.Value.Content;
    }

    public async Task<IFileService.TryGetResponse> TryGetFileAsync(string fileName, string path = "",
        CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(Path.Combine(_basePath, path, fileName));

        if (!await blobClient.ExistsAsync(token))
        {
            return new IFileService.TryGetResponse(false, Stream.Null);
        }

        var fileResponse = await blobClient.DownloadStreamingAsync(cancellationToken: token);
        return new IFileService.TryGetResponse(true, fileResponse.Value.Content);
    }

    public Task DeleteFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(Path.Combine(_basePath, path, fileName));
        return blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: token);
    }
}
