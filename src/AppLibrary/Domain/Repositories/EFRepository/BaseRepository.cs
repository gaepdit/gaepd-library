using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GaEpd.AppLibrary.Domain.Repositories.EFRepository;

/// <summary>
/// An implementation of <see cref="IRepository{TEntity,TKey}"/> using Entity Framework where TKey is
/// a <see cref="Guid"/> primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
public abstract class BaseRepository<TEntity, TContext>(TContext context)
    : BaseRepository<TEntity, Guid, DbContext>(context), IRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext;

/// <summary>
/// An implementation of <see cref="IRepository{TEntity,TKey}"/> using Entity Framework.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type for the entity.</typeparam>
/// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
[SuppressMessage("", "S2436")]
public abstract class BaseRepository<TEntity, TKey, TContext>(TContext context)
    : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
    public TContext Context => context;

    public async Task<TEntity> GetAsync(TKey id, CancellationToken token = default) =>
        await Context.Set<TEntity>().SingleOrDefaultAsync(entity => entity.Id.Equals(id), token).ConfigureAwait(false)
        ?? throw new EntityNotFoundException<TEntity>(id);

    public async Task<TEntity> GetAsync(TKey id, string[] includeProperties, CancellationToken token = default) =>
        await includeProperties.Aggregate(Context.Set<TEntity>().AsQueryable(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .SingleOrDefaultAsync(entity => entity.Id.Equals(id), token).ConfigureAwait(false)
        ?? throw new EntityNotFoundException<TEntity>(id);

    public Task<TEntity?> FindAsync(TKey id, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(entity => entity.Id.Equals(id), token);

    public Task<TEntity?> FindAsync(TKey id, string[] includeProperties, CancellationToken token = default) =>
        includeProperties.Aggregate(Context.Set<TEntity>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .SingleOrDefaultAsync(entity => entity.Id.Equals(id), token);

    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate, token);

    public async Task<IReadOnlyCollection<TEntity>> GetListAsync(CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().ToListAsync(token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<TEntity>> GetListAsync(string ordering, CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().OrderByIf(ordering).ToListAsync(token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync(token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate, string ordering, CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().Where(predicate)
            .OrderByIf(ordering).ToListAsync(token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate, PaginatedRequest paging, CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().Where(predicate)
            .OrderByIf(paging.Sorting).Skip(paging.Skip).Take(paging.Take).ToListAsync(token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate, PaginatedRequest paging, string[] includeProperties,
        CancellationToken token = default) =>
        await includeProperties.Aggregate(Context.Set<TEntity>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .Where(predicate).OrderByIf(paging.Sorting).Skip(paging.Skip).Take(paging.Take)
            .ToListAsync(token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        PaginatedRequest paging, CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking()
            .OrderByIf(paging.Sorting).Skip(paging.Skip).Take(paging.Take).ToListAsync(token).ConfigureAwait(false);

    public async Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        PaginatedRequest paging, string[] includeProperties, CancellationToken token = default) =>
        await includeProperties.Aggregate(Context.Set<TEntity>().AsNoTracking(),
                (queryable, includeProperty) => queryable.Include(includeProperty))
            .OrderByIf(paging.Sorting).Skip(paging.Skip).Take(paging.Take).ToListAsync(token).ConfigureAwait(false);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().CountAsync(predicate, token);

    public Task<bool> ExistsAsync(TKey id, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().AnyAsync(entity => entity.Id.Equals(id), token);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().AnyAsync(predicate, token);

    public async Task InsertAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, token).ConfigureAwait(false);
        if (autoSave) await SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        Context.Attach(entity);
        Context.Update(entity);

        if (!autoSave) return;

        try
        {
            await SaveChangesAsync(token).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await Context.Set<TEntity>().AsNoTracking()
                    .AnyAsync(e => e.Id.Equals(entity.Id), token).ConfigureAwait(false))
                throw new EntityNotFoundException<TEntity>(entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        Context.Set<TEntity>().Remove(entity);

        try
        {
            if (autoSave) await SaveChangesAsync(token).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await Context.Set<TEntity>().AsNoTracking()
                    .AnyAsync(e => e.Id.Equals(entity.Id), token).ConfigureAwait(false))
                throw new EntityNotFoundException<TEntity>(entity.Id);
            throw;
        }
    }

    public Task SaveChangesAsync(CancellationToken token = default) => Context.SaveChangesAsync(token);

    #region IDisposable, IAsyncDisposable

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
        if (_disposed) return;
        Context.Dispose();
        _disposed = true;
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual ValueTask DisposeAsyncCore() => Context.DisposeAsync();

    #endregion
}
