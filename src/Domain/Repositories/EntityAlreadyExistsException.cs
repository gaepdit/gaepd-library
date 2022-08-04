using System.Runtime.Serialization;

namespace GaEpd.Library.Domain.Repositories;

/// <summary>
/// The exception that is thrown if an entity is attempted to be inserted but already exists.
/// </summary>
[Serializable]
public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(Type entityType, object id)
        : base($"An Entity with that ID already exists. Entity type: {entityType.FullName}, id: {id}") { }

    protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
