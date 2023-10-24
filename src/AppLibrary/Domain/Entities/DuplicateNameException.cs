using System.Runtime.Serialization;

namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// The exception that is thrown if a named entity is added/updated with a name that already exists.
/// </summary>
[Serializable]
public class DuplicateNameException : Exception
{
    public DuplicateNameException(string name) : base($"An entity with that name already exists. Name: {name}") { }

    protected DuplicateNameException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
