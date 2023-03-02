using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Domain.Repositories;

public interface IRepository<TEntity, in TKey> :
    IReadRepository<TEntity, TKey>,
    IWriteRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{ }
