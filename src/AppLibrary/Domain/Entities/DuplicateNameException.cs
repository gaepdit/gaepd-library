namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// The exception that is thrown if a named entity is added/updated with a name that already exists.
/// </summary>
public class DuplicateNameException(string name) : Exception($"An entity with that name already exists. Name: {name}");
