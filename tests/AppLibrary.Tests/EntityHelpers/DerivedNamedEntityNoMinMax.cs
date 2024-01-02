using GaEpd.AppLibrary.Domain.Entities;

namespace AppLibrary.Tests.EntityHelpers;

public class DerivedNamedEntityNoMinMax(Guid id, string name) : StandardNamedEntity(id, name);
