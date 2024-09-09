using GaEpd.AppLibrary.Domain.Entities;
using System.Linq.Expressions;

namespace GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

/// <summary>
/// An implementation of <see cref="INamedEntityRepository{TEntity}"/> using in-memory data. The implementation is
/// derived from <see cref="BaseRepository{TEntity,TKey}"/> and uses a <see cref="Guid"/> for the Entity primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class NamedEntityRepository<TEntity>(IEnumerable<TEntity> items)
    : BaseRepository<TEntity>(items), INamedEntityRepository<TEntity>
    where TEntity : class, IEntity, INamedEntity
{
    public Task<TEntity?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(entity => string.Equals(entity.Name.ToUpper(), name.ToUpper())));

    public Task<IReadOnlyCollection<TEntity>> GetOrderedListAsync(CancellationToken token = default) =>
        Task.FromResult<IReadOnlyCollection<TEntity>>(
            Items.OrderBy(entity => entity.Name).ThenBy(entity => entity.Id).ToList());

    public Task<IReadOnlyCollection<TEntity>> GetOrderedListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default) =>
        Task.FromResult<IReadOnlyCollection<TEntity>>(
            Items.Where(predicate.Compile()).OrderBy(entity => entity.Name).ThenBy(entity => entity.Id).ToList());
}
