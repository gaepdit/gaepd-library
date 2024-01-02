namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="IAuditableEntity{TUserKey}"/>
/// and <see cref="ISoftDelete{TUserKey}"/>.
/// </summary>
public abstract class AuditableSoftDeleteEntity<TKey, TUserKey> : AuditableEntity<TKey, TUserKey>, ISoftDelete<TUserKey>
    where TKey : IEquatable<TKey>
    where TUserKey : IEquatable<TUserKey>
{
    protected AuditableSoftDeleteEntity() { }
    protected AuditableSoftDeleteEntity(TKey id) : base(id) { }

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
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="IAuditableEntity{TUserKey}"/>
/// and <see cref="ISoftDelete{TUserKey}"/>. A <see cref="string"/> is used for the User primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class AuditableSoftDeleteEntity<TKey> : AuditableSoftDeleteEntity<TKey, string>
    where TKey : IEquatable<TKey>
{
    protected AuditableSoftDeleteEntity() { }
    protected AuditableSoftDeleteEntity(TKey id) : base(id) { }
}

/// <summary>
/// The default implementation of <see cref="AuditableSoftDeleteEntity{TKey}"/> using <see cref="Guid"/> for the Entity
/// primary key, and <see cref="string"/> for the User primary key.
/// </summary>
public abstract class AuditableSoftDeleteEntity : AuditableSoftDeleteEntity<Guid>, IEntity
{
    protected AuditableSoftDeleteEntity() { }
    protected AuditableSoftDeleteEntity(Guid id) : base(id) { }
}
