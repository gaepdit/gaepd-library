namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// Defines an entity with a "Name" string property.
/// </summary>
public interface INamedEntity
{
    /// <summary>
    /// The Name of this entity.
    /// </summary>
    string Name { get; }
}
