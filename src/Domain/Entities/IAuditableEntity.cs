namespace GaEpd.Library.Domain.Entities;

/// <summary>
/// Marks an entity for enabling auditing in the database.
/// </summary>
public interface IAuditableEntity<TUserKey>
    where TUserKey : IEquatable<TUserKey>
{
    DateTimeOffset CreatedAt { get; }
    TUserKey? CreatedBy { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
    TUserKey? UpdatedBy { get; set; }
}
