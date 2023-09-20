using GaEpd.AppLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GaEpd.AppLibrary.Domain.Repositories.EFRepository;

/// <summary>
/// An implementation of <see cref="INamedEntityRepository{TEntity}"/> using Entity Framework. The implementation is
/// derived from <see cref="BaseRepository{TEntity,TKey}"/> and uses a <see cref="Guid"/> for the Entity primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class NamedEntityRepository<TEntity> : BaseRepository<TEntity, Guid>, INamedEntityRepository<TEntity>
    where TEntity : class, IEntity<Guid>, INamedEntity
{
    protected NamedEntityRepository(DbContext context) : base(context) { }

    public Task<TEntity?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.Set<TEntity>().AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);
}
