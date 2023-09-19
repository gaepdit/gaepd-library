using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Tests.EntityHelpers;

public class TestNamedEntityNoMinMax : StandardNamedEntity
{
    public TestNamedEntityNoMinMax(Guid id, string name) : base(id, name) { }
}
