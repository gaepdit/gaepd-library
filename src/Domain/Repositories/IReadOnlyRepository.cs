using GaEpd.Library.Domain.Entities;
using GaEpd.Library.Pagination;

namespace GaEpd.Library.Domain.Repositories;

/// <summary>
/// A generic repository for Entities with methods for reading data.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type for the entity.</typeparam>
public interface IReadOnlyRepository<TEntity, in TKey> : IDisposable
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Returns the <see cref="IEntity{TKey}"/> with the given <paramref name="id"/>.
    /// Throws <see cref="EntityNotFoundException"/> if no entity exists with the given Id.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An entity.</returns>
    Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the <see cref="IEntity{TKey}"/> with the given <paramref name="id"/>.
    /// Returns null if no entity exists with the given Id.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An entity.</returns>
    Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a single <see cref="IEntity{TKey}"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns null if there are no matches.
    /// Throws <see cref="InvalidOperationException"/> if there are multiple matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An entity.</returns>
    Task<TEntity?> FindAsync(
        Func<TEntity, bool> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a list of all <see cref="IEntity{TKey}"/> values>.
    /// Returns an empty list if none exist.
    /// </summary>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A list of entities.</returns>
    Task<IList<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a list of <see cref="IEntity{TKey}"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns an empty list if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A list of entities.</returns>
    Task<IList<TEntity>> GetListAsync(
        Func<TEntity, bool> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paginated list of <see cref="IEntity{TKey}"/> matching the conditions of the
    /// <paramref name="predicate"/>. Returns an empty list if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="paging">A <see cref="PaginatedRequest"/> to define the paging options.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A sorted and paged list of entities.</returns>
    Task<IList<TEntity>> GetPagedListAsync(
        Func<TEntity, bool> predicate,
        PaginatedRequest paging,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paginated list of all <see cref="IEntity{TKey}"/> values.
    /// Returns an empty list if there are no matches.
    /// </summary>
    /// <param name="paging">A <see cref="PaginatedRequest"/> to define the paging options.</param>
    /// <param name="cancellationToken"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A sorted and paged list of entities.</returns>
    Task<IList<TEntity>> GetPagedListAsync(
        PaginatedRequest paging,
        CancellationToken cancellationToken = default);
}
