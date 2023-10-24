namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// A manager for managing a <see cref="StandardNamedEntity"/>.
/// </summary>
public interface INamedEntityManager<TEntity> where TEntity : StandardNamedEntity
{
    /// <summary>
    /// Creates a new <see cref="StandardNamedEntity"/>.
    /// </summary>
    /// <param name="name">The name of the Entity to create.</param>
    /// <param name="createdById"></param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="DuplicateNameException">Thrown if an Entity already exists with the given name.</exception>
    /// <returns>The Entity that was created.</returns>
    Task<TEntity> CreateAsync(string name, string? createdById = null, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="StandardNamedEntity"/>.
    /// </summary>
    /// <param name="entity">The Entity to modify.</param>
    /// <param name="name">The new name for the Entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="DuplicateNameException">Thrown if an Entity already exists with the given name.</exception>
    Task ChangeNameAsync(TEntity entity, string name, CancellationToken token = default);
}
