namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// The exception that is thrown if a named entity is added/updated with a name that already exists.
/// </summary>
public class DuplicateNameException : Exception
{
    public DuplicateNameException(string name) : base($"An entity with that name already exists. Name: {name}") { }
}
