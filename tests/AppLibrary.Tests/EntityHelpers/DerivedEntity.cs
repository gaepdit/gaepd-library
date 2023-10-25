using GaEpd.AppLibrary.Domain.Entities;

namespace AppLibrary.Tests.EntityHelpers;

public class DerivedEntity : IEntity<Guid>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
