using GaEpd.Library.Domain.Entities;

namespace GaEpd.Library.Domain.Repositories;

/// <summary>
/// A generic repository for entities with methods for reading and writing.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type for the entity.</typeparam>
public interface IRepository<TEntity, in TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Inserts a new <see cref="IEntity{TKey}"/>.
    /// Throws <see cref="EntityAlreadyExistsException"/> if an entity already exists with the given Id.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <param name="autoSave">Whether to automatically save the changes.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The inserted entity.</returns>
    Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an <see cref="IEntity{TKey}"/>.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="autoSave">Whether to automatically save the changes.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The updated entity.</returns>
    Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an <see cref="IEntity{TKey}"/>.
    /// </summary>
    /// <param name="id">The Id of the entity to delete.</param>
    /// <param name="autoSave">Whether to automatically save the changes.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default);
}
