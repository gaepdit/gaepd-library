using GaEpd.AppLibrary.Domain.Repositories;

namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// An implementation of <see cref="INamedEntityManager{TEntity}"/>
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TRepository">The repository interface for managing the entity data.</typeparam>
public abstract class NamedEntityManager<TEntity, TRepository> : INamedEntityManager<TEntity>
    where TEntity : StandardNamedEntity, new()
    where TRepository : INamedEntityRepository<TEntity>
{
    private readonly TRepository _repository;
    protected NamedEntityManager(TRepository repository) => _repository = repository;

    public async Task<TEntity> CreateAsync(string name, string? createdById = null, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, ignoreId: null, token);
        var item = new TEntity();
        item.SetId(Guid.NewGuid());
        item.SetName(name);
        item.SetCreator(createdById);
        return item;
    }

    public async Task ChangeNameAsync(TEntity entity, string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, entity.Id, token);
        entity.SetName(name);
    }

    private async Task ThrowIfDuplicateName(string name, Guid? ignoreId, CancellationToken token)
    {
        // Validate the name is not a duplicate.
        var entity = await _repository.FindByNameAsync(name.Trim(), token);
        if (entity is not null && (ignoreId is null || entity.Id != ignoreId))
            throw new DuplicateNameException(name);
    }
}
