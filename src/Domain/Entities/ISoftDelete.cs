namespace GaEpd.Library.Domain.Entities;

/// <summary>
/// Marks an entity for enabling soft deletion in the database.
/// </summary>
public interface ISoftDelete<TUserKey>
    where TUserKey : IEquatable<TUserKey>
{
    bool IsDeleted { get; set; }
    DateTimeOffset? DeletedAt { get; set; }
    TUserKey? DeletedBy { get; set; }
}
