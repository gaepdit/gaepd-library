using GaEpd.AppLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GaEpd.AppLibrary.Domain.Repositories.EFRepository;

/// <summary>
/// An implementation of <see cref="INamedEntityRepository{TEntity}"/> using Entity Framework. The implementation is
/// derived from <see cref="BaseRepository{TEntity,TKey}"/> and uses a <see cref="Guid"/> for the Entity primary key.
/// The <see cref="DbContext"/> type is assumed, but a derivative may still be used.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class NamedEntityRepository<TEntity> : NamedEntityRepository<TEntity, DbContext>
    where TEntity : class, IEntity<Guid>, INamedEntity
{
    protected NamedEntityRepository(DbContext context) : base(context) { }
}

/// <summary>
/// An implementation of <see cref="INamedEntityRepository{TEntity}"/> using Entity Framework. The implementation is
/// derived from <see cref="BaseRepository{TEntity,TKey}"/> and uses a <see cref="Guid"/> for the Entity primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
public abstract class NamedEntityRepository<TEntity, TContext> :
    BaseRepository<TEntity, Guid, TContext>, INamedEntityRepository<TEntity>
    where TEntity : class, IEntity<Guid>, INamedEntity
    where TContext : DbContext
{
    protected NamedEntityRepository(TContext context) : base(context) { }

    public Task<TEntity?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);
}
