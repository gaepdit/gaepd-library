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
    public TUserKey? DeletedBy { get; private set; }

    /// <inheritdoc/>
    public void SetDeleted(TUserKey? userId)
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.Now;
        DeletedBy = userId;
    }
}
