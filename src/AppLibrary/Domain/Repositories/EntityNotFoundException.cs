namespace GaEpd.AppLibrary.Domain.Repositories;

/// <summary>
/// The exception that is thrown if an expected entity is not found. 
/// </summary>
public class EntityNotFoundException<T>(object id)
    : KeyNotFoundException($"Entity not found. Entity type: {typeof(T).FullName}, id: {id}");
