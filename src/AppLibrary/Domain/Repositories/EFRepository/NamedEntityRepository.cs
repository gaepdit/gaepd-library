using GaEpd.AppLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GaEpd.AppLibrary.Domain.Repositories.EFRepository;

/// <summary>
/// An implementation of <see cref="INamedEntityRepository{TEntity}"/> using Entity Framework. The implementation is
/// derived from <see cref="BaseRepository{TEntity,TContext}"/> and uses a <see cref="Guid"/> for the primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
public abstract class NamedEntityRepository<TEntity, TContext>(TContext context)
    : BaseRepository<TEntity, TContext>(context), INamedEntityRepository<TEntity>
    where TEntity : class, IEntity, INamedEntity
    where TContext : DbContext
{
    public Task<TEntity?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking()
            .SingleOrDefaultAsync(entity => string.Equals(entity.Name.ToUpper(), name.ToUpper()), token);

    public async Task<IReadOnlyCollection<TEntity>> GetOrderedListAsync(CancellationToken token = default) =>
        await Context.Set<TEntity>().AsNoTracking()
            .OrderBy(entity => entity.Name).ThenBy(entity => entity.Id)
            .ToListAsync(token).ConfigureAwait(false);
}
