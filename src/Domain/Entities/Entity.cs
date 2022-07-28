namespace GaEpd.Library.Domain.Entities;

/// <inheritdoc cref="IEntity{TKey}" />
[Serializable]
public abstract class Entity<TKey> : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <inheritdoc/>
    public TKey Id { get; protected set; } = default!;

    protected Entity() { }

    protected Entity(TKey id) => Id = id;
}
