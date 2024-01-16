namespace GaEpd.FileService;

public static class FileServiceImplementation
{
    public const string InMemory = nameof(InMemory);
    public const string FileSystem = nameof(FileSystem);
    public const string AzureBlobStorage = nameof(AzureBlobStorage);
}

public sealed class FileServiceSettings
{
    /// <summary>
    /// Determines which File Service implementation to use. Must be set to one of the values
    /// in <see cref="FileServiceImplementation"/>.
    /// </summary>
    public string FileService { get; init; } = string.Empty;

    // File System parameters
    public string FileSystemBasePath { get; init; } = string.Empty;
    public string NetworkUsername { get; init; } = string.Empty;
    public string NetworkDomain { get; init; } = string.Empty;
    public string NetworkPassword { get; init; } = string.Empty;

    // Azure Blob Storage parameters
    public string AzureAccountName { get; init; } = string.Empty;
    public string BlobContainer { get; init; } = string.Empty;
    public string BlobBasePath { get; init; } = string.Empty;
}
