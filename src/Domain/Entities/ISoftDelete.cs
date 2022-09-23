namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// Marks an entity for enabling soft deletion in the database.
/// </summary>
public interface ISoftDelete<TUserKey>
    where TUserKey : IEquatable<TUserKey>
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedAt { get; }
    TUserKey? DeletedBy { get; }

    /// <summary>
    /// Sets the <see cref="IsDeleted"/> property to "true", indicating that the element should be considered to have
    /// been deleted. Also sets the <see cref="DeletedBy"/> property to the <see cref="userId"/> parameter value and
    /// the <see cref="DeletedAt"/> property to the current <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="userId">The ID of the user deleting the entity.</param>
    void SetDeleted(TUserKey? userId);
}
