namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// Defines an entity with an "Id" primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public interface IEntity<out TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Id for this entity.
    /// </summary>
    TKey Id { get; }
}
