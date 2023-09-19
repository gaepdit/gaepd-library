using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Tests.EntityHelpers;

public class TestNamedEntity : StandardNamedEntity
{
    public const int MinLength = 4;
    public const int MaxLength = 9;

    public TestNamedEntity(Guid id, string name) : base(id, name)
    {
        MinNameLength = MinLength;
        MaxNameLength = MaxLength;
    }
}
