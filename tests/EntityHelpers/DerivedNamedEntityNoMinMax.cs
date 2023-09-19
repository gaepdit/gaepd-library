using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Tests.EntityHelpers;

public class DerivedNamedEntityNoMinMax : StandardNamedEntity
{
    public DerivedNamedEntityNoMinMax(Guid id, string name) : base(id, name) { }
}
