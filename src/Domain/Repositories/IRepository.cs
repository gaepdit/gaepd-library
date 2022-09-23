using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Domain.Repositories;

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
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <param name="autoSave">Whether to automatically save the changes.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The inserted entity.</returns>
    Task InsertAsync(TEntity entity, bool autoSave = false, CancellationToken token = default);

    /// <summary>
    /// Updates an <see cref="IEntity{TKey}"/>.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="autoSave">Whether to automatically save the changes.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The updated entity.</returns>
    Task UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken token = default);

    /// <summary>
    /// Deletes an <see cref="IEntity{TKey}"/>. Avoid using this method if the Entity 
    /// implements <see cref="ISoftDelete{TKey}"/>.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="autoSave">Whether to automatically save the changes.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken token = default);
}
