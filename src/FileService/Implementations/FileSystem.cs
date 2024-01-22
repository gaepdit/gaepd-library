using GaEpd.FileService.Utilities;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace GaEpd.FileService.Implementations;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class FileSystem : IFileService
{
    private readonly string _basePath;
    private readonly bool _useWindowsLogin;
    private readonly SafeAccessTokenHandle? _accessToken;

    /// <summary>
    /// Initializes a new instance of the FileSystem class.
    /// </summary>
    /// <param name="basePath">A path defining where the files will be stored.</param>
    public FileSystem(string basePath)
    {
        _basePath = Guard.NotNullOrWhiteSpace(basePath);
        Directory.CreateDirectory(_basePath);
    }

    /// <summary>
    /// Initializes a new instance of the FileSystem class. Use of this overload causes all file access operations
    /// to run as the provided WindowsIdentity and should only be used on Windows operating systems.
    /// </summary>
    /// <param name="basePath">A path defining where the files will be stored.</param>
    /// <param name="username">The username of the Windows Identity.</param>
    /// <param name="domain">The domain of the Windows Identity.</param>
    /// <param name="password">The password of the Windows Identity.</param>
    /// <exception cref="InvalidOperationException">Thrown if called on a non-Windows operating system.</exception>
    public FileSystem(string basePath, string username, string domain, string password)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new InvalidOperationException(
                "This FileSystem overload is only available on Windows operating systems.");

        _basePath = Guard.NotNullOrWhiteSpace(basePath);
        _basePath = Guard.NotNullOrWhiteSpace(username);

        _basePath = basePath;
        _useWindowsLogin = true;
        _accessToken = new WindowsLogin(username, domain, password).AccessToken;
        WindowsIdentity.RunImpersonated(_accessToken!, () => Directory.CreateDirectory(_basePath));
    }

    public Task SaveFileAsync(Stream stream, string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        return _useWindowsLogin
            ? WindowsIdentity.RunImpersonated(_accessToken!, SaveToFileSystem)
            : SaveToFileSystem();

        async Task SaveToFileSystem()
        {
            if (!string.IsNullOrEmpty(path)) Directory.CreateDirectory(Path.Combine(_basePath, path));
            var savePath = Path.Combine(_basePath, path, fileName);
            await using var fs = new FileStream(savePath, FileMode.Create);
            await stream.CopyToAsync(fs, token);
        }
    }

    public Task<bool> FileExistsAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        return _useWindowsLogin
            ? WindowsIdentity.RunImpersonated(_accessToken!, FileExists)
            : FileExists();

        Task<bool> FileExists() => Task.FromResult(File.Exists(Path.Combine(_basePath, path, fileName)));
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async IAsyncEnumerable<IFileService.FileDescription> GetFilesAsync(string path = "",
        [EnumeratorCancellation] CancellationToken token = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        var fullBasePath = new DirectoryInfo(_basePath).FullName;

        foreach (var fileInfo in new DirectoryInfo(Path.Combine(_basePath, path))
                     .EnumerateFiles("*", new EnumerationOptions { RecurseSubdirectories = true }))
        {
            yield return new IFileService.FileDescription
            {
                FullName = fileInfo.FullName.Replace($"{fullBasePath}{Path.DirectorySeparatorChar}", string.Empty),
                ContentLength = fileInfo.Length,
                CreatedOn = fileInfo.CreationTimeUtc,
            };
        }
    }

    public Task<Stream> GetFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        return _useWindowsLogin
            ? WindowsIdentity.RunImpersonated(_accessToken!, GetFileStream)
            : GetFileStream();

        Task<Stream> GetFileStream()
        {
            try
            {
                return Task.FromResult<Stream>(File.OpenRead(Path.Combine(_basePath, path, fileName)));
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new FileNotFoundException(Path.Combine(path, fileName), e);
            }
        }
    }

    public Task<IFileService.TryGetResponse> TryGetFileAsync(string fileName, string path = "",
        CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        var filePath = Path.Combine(_basePath, path, fileName);
        return _useWindowsLogin
            ? WindowsIdentity.RunImpersonated(_accessToken!, TryGetFileInternal)
            : TryGetFileInternal();

        Task<IFileService.TryGetResponse> TryGetFileInternal()
        {
            return Task.FromResult(File.Exists(filePath)
                ? new IFileService.TryGetResponse(File.OpenRead(filePath))
                : IFileService.TryGetResponse.FailedTryGetResponse);
        }
    }

    public Task DeleteFileAsync(string fileName, string path = "", CancellationToken token = default)
    {
        Guard.NotNullOrWhiteSpace(fileName);
        return _useWindowsLogin
            ? WindowsIdentity.RunImpersonated(_accessToken!, DeleteFromFileSystem)
            : DeleteFromFileSystem();

        Task DeleteFromFileSystem()
        {
            File.Delete(Path.Combine(_basePath, path, fileName));
            return Task.CompletedTask;
        }
    }
}
