using GaEpd.AppLibrary.Domain.Entities;

namespace AppLibrary.Tests.EntityTests;

public class TestNamedEntityNoMinMax(Guid id, string name) : StandardNamedEntity(id, name);
