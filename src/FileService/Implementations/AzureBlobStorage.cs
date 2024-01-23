using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GaEpd.FileService.Utilities;
using System.Runtime.CompilerServices;

namespace GaEpd.FileService.Implementations;

public class AzureBlobStorage : IFileService
{
    private readonly string _basePath;
    private readonly BlobContainerClient _containerClient;

    /// <summary>
    /// Initializes a new instance of the AzureBlobStorage class.
    /// </summary>
    /// <param name="accountName">The Azure storage account name.</param>
    /// <param name="container">The blob storage container.</param>
    /// <param name="basePath">An optional path defining where the files will be stored within the container.</param>
    public AzureBlobStorage(string accountName, string container, string basePath = "")
    {
        Guard.NotNullOrWhiteSpace(accountName);
        Guard.NotNullOrWhiteSpace(container);
        _basePath = basePath;
        var serviceUri = new Uri($"https://{accountName}.blob.core.windows.net");
        var serviceClient = new BlobServiceClient(serviceUri, new DefaultAzureCredential());
        _containerClient = serviceClient.GetBlobContainerClient(container);
    }

    public async Task SaveFileAsync(Stream stream, string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(PathTool.Combine(_basePath, path, fileName));
        if (stream.CanSeek) stream.Position = 0;
        await blobClient.UploadAsync(stream, overwrite: true, token);
    }

    public async Task<bool> FileExistsAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(PathTool.Combine(_basePath, path, fileName));
        return (await blobClient.ExistsAsync(token)).Value;
    }

    public async IAsyncEnumerable<IFileService.FileDescription> GetFilesAsync(string path = "",
        [EnumeratorCancellation] CancellationToken token = default)
    {
        var blobItems = _containerClient.GetBlobsAsync(
            prefix: PathTool.CombineWithDirectorySeparator(_basePath, path), cancellationToken: token);

        await foreach (var blobItem in blobItems)
        {
            yield return new IFileService.FileDescription
            {
                FullName = blobItem.Name.Replace(PathTool.CombineWithDirectorySeparator(_basePath), string.Empty),
                ContentLength = blobItem.Properties.ContentLength,
                CreatedOn = blobItem.Properties.CreatedOn,
            };
        }
    }

    public async Task<Stream> GetFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(PathTool.Combine(_basePath, path, fileName));
        if (!await blobClient.ExistsAsync(token)) throw new FileNotFoundException(PathTool.Combine(path, fileName));
        return (await blobClient.DownloadStreamingAsync(cancellationToken: token)).Value.Content;
    }

    public async Task<IFileService.TryGetResponse> TryGetFileAsync(string fileName, string path = "",
        CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(PathTool.Combine(_basePath, path, fileName));

        if (!await blobClient.ExistsAsync(token))
            return IFileService.TryGetResponse.FailedTryGetResponse;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: token);
        return new IFileService.TryGetResponse(response.Value.Content);
    }

    public Task DeleteFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var blobClient = _containerClient.GetBlobClient(PathTool.Combine(_basePath, path, fileName));
        return blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: token);
    }
}
