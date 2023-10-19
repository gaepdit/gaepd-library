namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// Marks an entity for enabling auditing in the database.
/// </summary>
public interface IAuditableEntity<TUserKey>
    where TUserKey : IEquatable<TUserKey>
{
    DateTimeOffset? CreatedAt { get; }
    TUserKey? CreatedById { get; }
    DateTimeOffset? UpdatedAt { get; }
    TUserKey? UpdatedById { get; }

    /// <summary>
    /// Sets the <see cref="CreatedById"/> property to the <see cref="userId"/> parameter value and sets
    /// the <see cref="CreatedAt"/> property to the current <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="userId">The ID of the user creating the entity.</param>
    void SetCreator(TUserKey? userId);

    /// <summary>
    /// Sets the <see cref="UpdatedById"/> property to the <see cref="userId"/> parameter value and sets
    /// the <see cref="UpdatedAt"/> property to the current <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="userId">The ID of the user updating the entity.</param>
    void SetUpdater(TUserKey? userId);
}
