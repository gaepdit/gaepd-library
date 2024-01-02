namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="ISoftDelete{TUserKey}"/>.
/// </summary>
public abstract class SoftDeleteEntity<TKey, TUserKey> : Entity<TKey>, ISoftDelete<TUserKey>
    where TKey : IEquatable<TKey>
    where TUserKey : IEquatable<TUserKey>
{
    protected SoftDeleteEntity() { }
    protected SoftDeleteEntity(TKey id) : base(id) { }

    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public TUserKey? DeletedById { get; private set; }

    public void SetDeleted(TUserKey? userId)
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.Now;
        DeletedById = userId;
    }

    public void SetNotDeleted()
    {
        IsDeleted = false;
        DeletedAt = default;
        DeletedById = default;
    }
}

/// <summary>
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="ISoftDelete{TUserKey}"/>
/// A <see cref="string"/> is used for the User primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class SoftDeleteEntity<TKey> : SoftDeleteEntity<TKey, string>
    where TKey : IEquatable<TKey>
{
    protected SoftDeleteEntity() { }
    protected SoftDeleteEntity(TKey id) : base(id) { }
}

/// <summary>
/// The default implementation of <see cref="SoftDeleteEntity{TKey}"/> using <see cref="Guid"/> for the Entity
/// primary key, and <see cref="string"/> for the User primary key.
/// </summary>
public abstract class SoftDeleteEntity : SoftDeleteEntity<Guid>, IEntity
{
    protected SoftDeleteEntity() { }
    protected SoftDeleteEntity(Guid id) : base(id) { }
}
