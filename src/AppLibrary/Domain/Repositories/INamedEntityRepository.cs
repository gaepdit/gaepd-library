using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Domain.Repositories;

/// <summary>
/// A repository for working with entities that have a <see cref="INamedEntity.Name"/> property.  
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface INamedEntityRepository<TEntity> : IRepository<TEntity>
    where TEntity : IEntity, INamedEntity
{
    /// <summary>
    /// Returns the <see cref="TEntity"/> with the given <paramref name="name"/>.
    /// Returns null if the name does not exist.
    /// </summary>
    /// <param name="name">The Name of the SimpleNamedEntity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A SimpleNamedEntity entity.</returns>
    Task<TEntity?> FindByNameAsync(string name, CancellationToken token = default);
    /// <summary>
    /// Returns a read-only collection of all <see cref="IEntity{TKey}"/> values.
    /// Returns an empty collection if none exist.
    /// </summary>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of entities.</returns>
    Task<IReadOnlyCollection<TEntity>> GetOrderedListAsync(CancellationToken token = default);
}
