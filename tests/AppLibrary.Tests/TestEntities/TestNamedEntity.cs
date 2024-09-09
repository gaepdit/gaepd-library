using GaEpd.AppLibrary.Domain.Entities;

namespace AppLibrary.Tests.TestEntities;

public class TestNamedEntity : StandardNamedEntity
{
    public override int MinNameLength => 4;
    public override int MaxNameLength => 9;

    public TestNamedEntity() { }
    public TestNamedEntity(Guid id, string name) : base(id, name) { }
}
