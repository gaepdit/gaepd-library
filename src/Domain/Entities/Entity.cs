using GaEpd.AppLibrary.GuardClauses;

namespace GaEpd.AppLibrary.Domain.Entities;

/// <inheritdoc cref="IEntity{TKey}" />
[Serializable]
public abstract class Entity<TKey> : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private TKey? _id;

    /// <inheritdoc/>
    public TKey Id
    {
        protected set => _id = Guard.NotNull(value);
        get => _id ?? throw new InvalidOperationException($"Uninitialized property: {nameof(Id)}");
    }

    protected Entity() { }

    protected Entity(TKey id) => Id = id;
}
