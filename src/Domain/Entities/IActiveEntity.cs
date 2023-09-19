namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// Defines an entity with an "Active" boolean property.
/// </summary>
public interface IActiveEntity
{
    /// <summary>
    /// A flag indicating whether the entity is Active or not.
    /// </summary>
    bool Active { get; }
}
