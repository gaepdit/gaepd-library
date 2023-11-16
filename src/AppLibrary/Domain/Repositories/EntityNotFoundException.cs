namespace GaEpd.AppLibrary.Domain.Repositories;

/// <summary>
/// The exception that is thrown if an expected entity is not found. 
/// </summary>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type entityType, object id)
        : base($"Entity not found. Entity type: {entityType.FullName}, id: {id}") { }
}
