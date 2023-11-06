using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Domain.Repositories;

/// <summary>
/// A generic repository for entities with methods for writing data.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type for the entity.</typeparam>
public interface IWriteRepository<in TEntity, in TKey> : IDisposable, IAsyncDisposable
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Inserts a new <see cref="IEntity{TKey}"/>.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <param name="autoSave">Whether to automatically save the changes (default is true).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task InsertAsync(TEntity entity, bool autoSave = true, CancellationToken token = default);

    /// <summary>
    /// Updates an <see cref="IEntity{TKey}"/>.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="autoSave">Whether to automatically save the changes (default is true).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken token = default);

    /// <summary>
    /// Deletes an <see cref="IEntity{TKey}"/>. Avoid using this method if the Entity 
    /// implements <see cref="ISoftDelete{TKey}"/>.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="autoSave">Whether to automatically save the changes (default is true).</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken token = default);

    /// <summary>
    /// Saves all changes to the repository. Only use by repositories that require explicit calls to save changes
    /// and when the autoSave parameter is set to false in one of the other methods.
    /// </summary>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task SaveChangesAsync(CancellationToken token = default);
}
