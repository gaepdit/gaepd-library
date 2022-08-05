namespace GaEpd.Library.Domain.Entities;

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

    public void SetCreator(TUserKey? userId)
    {
        CreatedAt = DateTimeOffset.Now;
        CreatedBy = userId;
    }

    public void SetUpdater(TUserKey? userId)
    {
        UpdatedAt = DateTimeOffset.Now;
        UpdatedBy = userId;
    }
}
