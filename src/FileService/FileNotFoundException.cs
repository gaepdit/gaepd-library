using System.Runtime.Serialization;

namespace GaEpd.FileService;

/// <summary>
/// The exception that is thrown if a requested file is not found. 
/// </summary>
[Serializable]
public class FileNotFoundException : Exception
{
    public FileNotFoundException(string filePath, Exception? innerException = null)
        : base($"File '{filePath}' not found", innerException) { }

    protected FileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
