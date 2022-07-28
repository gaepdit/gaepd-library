namespace GaEpd.Library.Domain.Entities;

/// <summary>
/// An implementation of <see cref="IEntity{TKey}"/> that also implements <see cref="IAuditableEntity{TUserKey}"/>.
/// </summary>
public abstract class AuditableEntity<TKey, TUserKey> : Entity<TKey>, IAuditableEntity<TUserKey>
    where TKey : IEquatable<TKey>
    where TUserKey : IEquatable<TUserKey>
{
    public DateTimeOffset CreatedAt { get; set; }
    public TUserKey? CreatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public TUserKey? UpdatedBy { get; set; }

    protected AuditableEntity() { }
    protected AuditableEntity(TKey id) : base(id) { }
}
