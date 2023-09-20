using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Tests.EntityHelpers;

public class DerivedNamedEntity : StandardNamedEntity
{
    public override int MinNameLength => 4;
    public override int MaxNameLength => 9;

    public DerivedNamedEntity() { }
    public DerivedNamedEntity(Guid id, string name) : base(id, name) { }
}
