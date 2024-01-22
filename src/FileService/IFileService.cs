namespace GaEpd.FileService;

/// <summary>
/// A service for managing file persistence.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Saves a <see cref="Stream"/> as a file. If the file already exists, overwrites the existing file.
    /// </summary>
    /// <param name="stream">The Stream to save.</param>
    /// <param name="fileName">The file name to save the Stream as.</param>
    /// <param name="path">The location where to save the Stream.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task SaveFileAsync(Stream stream, string fileName, string path = "", CancellationToken token = default);

    /// <summary>
    /// Checks if the specified file exists.
    /// </summary>
    /// <param name="fileName">The name of the file to look for.</param>
    /// <param name="path">The location of the file.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>`true` if the file exists; otherwise `false`.</returns>
    Task<bool> FileExistsAsync(string fileName, string path = "", CancellationToken token = default);

    /// <summary>
    /// Recursively lists files in a specified path and its subfolders. The order of files returned depends on
    /// the implementation and should not be relied on to be consistent.
    /// </summary>
    /// <param name="path">The location of the files to return.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An <see cref="IAsyncEnumerable{FileDescription}"/> collection of files in the specified path.
    /// If no files exist in the specified path, returns an empty collection.</returns>
    IAsyncEnumerable<FileDescription> GetFilesAsync(string path = "", CancellationToken token = default);

    /// <summary>
    /// Retrieves a file as a <see cref="Stream"/>.
    /// </summary>
    /// <param name="fileName">The name of the file to retrieve.</param>
    /// <param name="path">The location of the file to retrieve.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="FileNotFoundException">The file does not exist.</exception>
    /// <returns>A Stream.</returns>
    Task<Stream> GetFileAsync(string fileName, string path = "", CancellationToken token = default);

    /// <summary>
    /// Retrieves a file as a <see cref="Stream"/>. If file retrieval succeeds, this method returns a
    /// <see cref="TryGetResponse"/> with the `Value` equal to the requested Stream and `Success` equal
    /// to `true. Otherwise, `Value` contains <see cref="Stream.Null"/> and `Success` equals `false`.
    /// </summary>
    /// <param name="fileName">The name of the file to retrieve.</param>
    /// <param name="path">The location of the file to retrieve.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A <see cref="TryGetResponse"/> with `Success` = `true` if the file is found or `false` if the file
    /// is not found.</returns>
    Task<TryGetResponse> TryGetFileAsync(string fileName, string path = "", CancellationToken token = default);

    /// <summary>
    /// Deletes a file. If the file does not exist, the method returns without throwing an exception.
    /// </summary>
    /// <param name="fileName">The name of the file to delete.</param>
    /// <param name="path">The location of the file to delete.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    Task DeleteFileAsync(string fileName, string path = "", CancellationToken token = default);

    // Return types

    /// <summary>
    /// Contains the results of a call to <see cref="IFileService.TryGetFileAsync"/>.
    /// </summary>
    public sealed record TryGetResponse(Stream Value) : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Success is true if the requested file is found; otherwise false.
        /// </summary>
        public bool Success { get; private init; } = true;

        public void Dispose() => Value.Dispose();
        public async ValueTask DisposeAsync() => await Value.DisposeAsync();

        public static TryGetResponse FailedTryGetResponse => new(Stream.Null) { Success = false };
    }

    /// <summary>
    /// Contains information about a file.
    /// </summary>
    public sealed record FileDescription
    {
        /// <summary>
        /// Path and filename.
        /// </summary>
        public required string FullName { get; init; }

        /// <summary>
        /// Size in bytes.
        /// </summary>
        public long? ContentLength { get; init; }

        /// <summary>
        /// Created.
        /// </summary>
        public DateTimeOffset? CreatedOn { get; init; }
    }
}
