namespace GaEpd.FileService;

/// <summary>
/// The exception that is thrown if a requested file is not found. 
/// </summary>
public class FileNotFoundException : Exception
{
    public FileNotFoundException(string filePath, Exception? innerException = null)
        : base($"File '{filePath}' not found", innerException) { }
}
