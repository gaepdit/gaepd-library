using GaEpd.FileService.Utilities;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace GaEpd.FileService.Implementations;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public class FileSystem : IFileService
{
    private readonly string _basePath;
    private readonly bool _useWindowsLogin;
    private readonly SafeAccessTokenHandle? _accessToken;

    public FileSystem(string basePath)
    {
        _basePath = Guard.NotNullOrWhiteSpace(basePath);
        Directory.CreateDirectory(_basePath);
    }

    public FileSystem(string basePath, string username, string domain, string password)
    {
        _basePath = Guard.NotNullOrWhiteSpace(basePath);
        if (username == string.Empty || password == string.Empty)
        {
            Directory.CreateDirectory(_basePath);
            return;
        }

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) throw new InvalidOperationException();
        _accessToken = new WindowsLogin(username, domain, password).AccessToken;
        _basePath = basePath;
        _useWindowsLogin = true;
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
            ? WindowsIdentity.RunImpersonated(_accessToken!, InternalTryGetFile)
            : InternalTryGetFile();

        Task<IFileService.TryGetResponse> InternalTryGetFile()
        {
            return Task.FromResult(File.Exists(filePath)
                ? new IFileService.TryGetResponse(true, File.OpenRead(filePath))
                : new IFileService.TryGetResponse(false, Stream.Null));
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
