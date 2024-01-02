namespace GaEpd.FileService;

/// <summary>
/// The exception that is thrown if a requested file is not found. 
/// </summary>
public class FileNotFoundException(string filePath, Exception? innerException = null) :
    Exception($"File '{filePath}' not found", innerException);
