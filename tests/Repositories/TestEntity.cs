using GaEpd.AppLibrary.Domain.Entities;

namespace GaEpd.AppLibrary.Tests.RepositoryHelpers;

public class TestEntity : IEntity<Guid>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
