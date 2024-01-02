using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace GaEpd.AppLibrary.Domain.Repositories;

/// <summary>
/// A generic repository for Entities with methods for reading data.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type for the entity.</typeparam>
public interface IReadRepository<TEntity, in TKey> : IDisposable, IAsyncDisposable
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Returns the <see cref="IEntity{TKey}"/> with the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{TEntity}">Thrown if no entity exists with the given Id.</exception>
    /// <returns>An entity.</returns>
    Task<TEntity> GetAsync(TKey id, CancellationToken token = default);

    /// <summary>
    /// Returns the <see cref="IEntity{TKey}"/> with the given <paramref name="id"/>.
    /// Returns null if no entity exists with the given Id.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An entity or null.</returns>
    Task<TEntity?> FindAsync(TKey id, CancellationToken token = default);

    /// <summary>
    /// Returns a single <see cref="IEntity{TKey}"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns null if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="InvalidOperationException">Thrown if there are multiple matches.</exception>
    /// <returns>An entity or null.</returns>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

    /// <summary>
    /// Returns a read-only collection of all <see cref="IEntity{TKey}"/> values.
    /// Returns an empty collection if none exist.
    /// </summary>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of entities.</returns>
    Task<IReadOnlyCollection<TEntity>> GetListAsync(CancellationToken token = default);

    /// <summary>
    /// Returns a read-only collection of <see cref="IEntity{TKey}"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of entities.</returns>
    Task<IReadOnlyCollection<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default);

    /// <summary>
    /// Returns a paginated read-only collection of <see cref="IEntity{TKey}"/> matching the conditions of the
    /// <paramref name="predicate"/>. Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="paging">A <see cref="PaginatedRequest"/> to define the paging options.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A sorted and paged read-only collection of entities.</returns>
    Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        PaginatedRequest paging,
        CancellationToken token = default);

    /// <summary>
    /// Returns a paginated read-only collection of all <see cref="IEntity{TKey}"/> values.
    /// Returns an empty collection if there are no matches.
    /// </summary>
    /// <param name="paging">A <see cref="PaginatedRequest"/> to define the paging options.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A sorted and paged read-only collection of entities.</returns>
    Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(PaginatedRequest paging, CancellationToken token = default);

    /// <summary>
    /// Returns the count of <see cref="IEntity{TKey}"/> matching the conditions of the <paramref name="predicate"/>.
    /// Returns zero if there are no matches.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>The number of matching entities.</returns>
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

    /// <summary>
    /// Returns a boolean indicating whether an <see cref="IEntity{TKey}"/> with the given <paramref name="id"/> exists.
    /// </summary>
    /// <param name="id">The Id of the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>True if the entity exists; otherwise false.</returns>
    public Task<bool> ExistsAsync(TKey id, CancellationToken token = default);

    /// <summary>
    /// Returns a boolean indicating whether an <see cref="IEntity{TKey}"/> exists matching the conditions of
    /// the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">The search conditions.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>True if the entity exists; otherwise false.</returns>
    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
}
