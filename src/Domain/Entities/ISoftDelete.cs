namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// Marks an entity for enabling soft deletion in the database.
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedAt { get; }
}

/// <summary>
/// Marks an entity for enabling soft deletion in the database and records the ID of the user who deleted the entity.
/// </summary>
public interface ISoftDelete<TUserKey> : ISoftDelete
    where TUserKey : IEquatable<TUserKey>
{
    TUserKey? DeletedById { get; }

    /// <summary>
    /// Sets the <see cref="ISoftDelete.IsDeleted"/> property to "true", indicating that the element should be considered to have
    /// been deleted. Also sets the <see cref="DeletedById"/> property to the <see cref="userId"/> parameter value and
    /// the <see cref="ISoftDelete.DeletedAt"/> property to the current <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="userId">The ID of the user deleting the entity.</param>
    void SetDeleted(TUserKey? userId);
}
