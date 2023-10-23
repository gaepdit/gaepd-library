namespace GaEpd.AppLibrary.Domain.Entities;

[Serializable]
public abstract class Entity<TKey> : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private TKey? _id;

    public TKey Id
    {
        protected set => _id = Guard.NotNull(value);
        get => _id ?? throw new InvalidOperationException($"Uninitialized property: {nameof(Id)}");
    }

    protected Entity() { }
    protected Entity(TKey id) => Id = id;
}
