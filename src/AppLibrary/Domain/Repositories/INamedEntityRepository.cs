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
}
