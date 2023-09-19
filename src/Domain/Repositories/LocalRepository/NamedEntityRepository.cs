using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Domain.Repositories.LocalRepository;

public abstract class NamedEntityRepository<TEntity> : BaseRepository<TEntity, Guid>, INamedEntityRepository<TEntity>
    where TEntity : IEntity<Guid>, INamedEntity
{
    protected NamedEntityRepository(IEnumerable<TEntity> items) : base(items) { }

    public Task<TEntity?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));
}
