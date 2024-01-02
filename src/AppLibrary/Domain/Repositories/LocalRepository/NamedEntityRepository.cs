using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

/// <summary>
/// An implementation of <see cref="INamedEntityRepository{TEntity}"/> using in-memory data. The implementation is
/// derived from <see cref="BaseRepository{TEntity,TKey}"/> and uses a <see cref="Guid"/> for the Entity primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class NamedEntityRepository<TEntity>(IEnumerable<TEntity> items) :
    BaseRepository<TEntity, Guid>(items), INamedEntityRepository<TEntity>
    where TEntity : IEntity<Guid>, INamedEntity
{
    public Task<TEntity?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));
}
