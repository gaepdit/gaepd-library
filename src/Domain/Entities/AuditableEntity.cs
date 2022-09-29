namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="IAuditableEntity{TUserKey}"/>.
/// </summary>
public abstract class AuditableEntity<TKey, TUserKey> : Entity<TKey>, IAuditableEntity<TUserKey>
    where TKey : IEquatable<TKey>
    where TUserKey : IEquatable<TUserKey>
{
    protected AuditableEntity() { }
    protected AuditableEntity(TKey id) : base(id) { }

    public DateTimeOffset? CreatedAt { get; private set; }
    public TUserKey? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public TUserKey? UpdatedBy { get; private set; }

    /// <inheritdoc/>
    public void SetCreator(TUserKey? userId)
    {
        CreatedAt = DateTimeOffset.Now;
        CreatedBy = userId;
    }

    /// <inheritdoc/>
    public void SetUpdater(TUserKey? userId)
    {
        UpdatedAt = DateTimeOffset.Now;
        UpdatedBy = userId;
    }
}

/// <summary>
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="IAuditableEntity{TUserKey}"/>.
/// A <see cref="string"/> is used for the User primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class AuditableEntity<TKey> : AuditableEntity<TKey, string>
    where TKey : IEquatable<TKey>
{
    protected AuditableEntity() { }
    protected AuditableEntity(TKey id) : base(id) { }
}

/// <summary>
/// The default implementation of <see cref="AuditableEntity{TKey}"/> using <see cref="Guid"/> for the Entity primary
/// key, and <see cref="string"/> for the User primary key.
/// </summary>
public abstract class AuditableEntity : AuditableEntity<Guid>
{
    protected AuditableEntity() { }
    protected AuditableEntity(Guid id) : base(id) { }
}
