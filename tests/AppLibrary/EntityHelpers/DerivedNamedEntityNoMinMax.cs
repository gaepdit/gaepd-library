using GaEpd.AppLibrary.Domain.Entities;

namespace AppLibrary.Tests.EntityHelpers;

public class DerivedNamedEntityNoMinMax : StandardNamedEntity
{
    public DerivedNamedEntityNoMinMax(Guid id, string name) : base(id, name) { }
}
