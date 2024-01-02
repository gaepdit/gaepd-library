using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using System.Linq.Expressions;

namespace GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

/// <summary>
/// An implementation of <see cref="IRepository{TEntity,TKey}"/> using in-memory data where TKey is
/// a <see cref="Guid"/> primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class BaseRepository<TEntity>(IEnumerable<TEntity> items)
    : BaseRepository<TEntity, Guid>(items), IRepository<TEntity>
    where TEntity : class, IEntity;

/// <summary>
/// An implementation of <see cref="IRepository{TEntity,TKey}"/> using in-memory data.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type for the entity.</typeparam>
public abstract class BaseRepository<TEntity, TKey>(IEnumerable<TEntity> items)
    : IRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public ICollection<TEntity> Items { get; } = items.ToList();

    public Task<TEntity> GetAsync(TKey id, CancellationToken token = default) =>
        Items.Any(e => e.Id.Equals(id))
            ? Task.FromResult(Items.Single(e => e.Id.Equals(id)))
            : throw new EntityNotFoundException<TEntity>(id);

    public Task<TEntity?> FindAsync(TKey id, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Id.Equals(id)));

    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(predicate.Compile()));

    public Task<IReadOnlyCollection<TEntity>> GetListAsync(CancellationToken token = default) =>
        Task.FromResult(Items.ToList() as IReadOnlyCollection<TEntity>);

    public Task<IReadOnlyCollection<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default) =>
        Task.FromResult(Items.Where(predicate.Compile()).ToList() as IReadOnlyCollection<TEntity>);

    public Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate,
        PaginatedRequest paging, CancellationToken token = default)
    {
        var result = Items.Where(predicate.Compile()).AsQueryable()
            .OrderByIf(paging.Sorting)
            .Skip(paging.Skip).Take(paging.Take);
        return Task.FromResult(result.ToList() as IReadOnlyCollection<TEntity>);
    }

    public Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(PaginatedRequest paging,
        CancellationToken token = default)
    {
        var result = Items.AsQueryable()
            .OrderByIf(paging.Sorting)
            .Skip(paging.Skip).Take(paging.Take);
        return Task.FromResult(result.ToList() as IReadOnlyCollection<TEntity>);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.Count(predicate.Compile()));

    public Task<bool> ExistsAsync(TKey id, CancellationToken token = default) =>
        Task.FromResult(Items.Any(e => e.Id.Equals(id)));

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Task.FromResult(Items.Any(predicate.Compile()));

    public Task InsertAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        var item = await GetAsync(entity.Id, token);
        Items.Remove(item);
        Items.Add(entity);
    }

    public async Task DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken token = default) =>
        Items.Remove(await GetAsync(entity.Id, token));

    // Local repository does not require changes to be explicitly saved.
    public Task SaveChangesAsync(CancellationToken token = default) => Task.CompletedTask;

    #region IDisposable,  IAsyncDisposable

    private bool _disposed;
    ~BaseRepository() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(obj: this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(disposing: false);
        GC.SuppressFinalize(obj: this);
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    // ReSharper disable once UnusedParameter.Global
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed) _disposed = true;
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;

    #endregion
}
