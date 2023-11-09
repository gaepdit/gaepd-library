using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace GaEpd.AppLibrary.Domain.Repositories.EFRepository;

/// <summary>
/// An implementation of <see cref="IRepository{TEntity,TKey}"/> using Entity Framework. The <see cref="DbContext"/>
/// type is assumed, but a derivative may still be used.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type for the entity.</typeparam>
public abstract class BaseRepository<TEntity, TKey> : BaseRepository<TEntity, TKey, DbContext>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected BaseRepository(DbContext context) : base(context) { }
}

/// <summary>
/// An implementation of <see cref="IRepository{TEntity,TKey}"/> using Entity Framework.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type for the entity.</typeparam>
/// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
[SuppressMessage("", "S2436")]
public abstract class BaseRepository<TEntity, TKey, TContext> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
    public readonly TContext Context;

    protected BaseRepository(TContext context) => Context = context;

    public async Task<TEntity> GetAsync(TKey id, CancellationToken token = default) =>
        await Context.Set<TEntity>().SingleOrDefaultAsync(e => e.Id.Equals(id), token)
        ?? throw new EntityNotFoundException(typeof(TEntity), id);

    public Task<TEntity?> FindAsync(TKey id, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(e => e.Id.Equals(id), token);

    public Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(predicate, token);

    public async Task<IReadOnlyCollection<TEntity>> GetListAsync(CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().ToListAsync(token);

    public async Task<IReadOnlyCollection<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync(token);

    public async Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate, PaginatedRequest paging, CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking().Where(predicate)
            .OrderByIf(paging.Sorting).Skip(paging.Skip).Take(paging.Take).ToListAsync(token);

    public async Task<IReadOnlyCollection<TEntity>> GetPagedListAsync(
        PaginatedRequest paging, CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking()
            .OrderByIf(paging.Sorting).Skip(paging.Skip).Take(paging.Take).ToListAsync(token);

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().CountAsync(predicate, token);

    public Task<bool> ExistsAsync(TKey id, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Id.Equals(id), token);

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking().AnyAsync(predicate, token);

    public async Task InsertAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, token);
        if (autoSave) await SaveChangesAsync(token);
    }

    public async Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        Context.Attach(entity);
        Context.Update(entity);

        if (!autoSave) return;

        try
        {
            await SaveChangesAsync(token);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await Context.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Id.Equals(entity.Id), token))
                throw new EntityNotFoundException(typeof(TEntity), entity.Id);
            throw;
        }
    }

    public async Task DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken token = default)
    {
        Context.Set<TEntity>().Remove(entity);

        try
        {
            if (autoSave) await SaveChangesAsync(token);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await Context.Set<TEntity>().AsNoTracking().AnyAsync(e => e.Id.Equals(entity.Id), token))
            {
                throw new EntityNotFoundException(typeof(TEntity), entity.Id);
            }

            throw;
        }
    }

    public async Task SaveChangesAsync(CancellationToken token = default) => await Context.SaveChangesAsync(token);

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
        if (_disposed) return;
        Context.Dispose();
        _disposed = true;
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual async ValueTask DisposeAsyncCore() => 
        await Context.DisposeAsync().ConfigureAwait(false);

    #endregion
}
