using GaEpd.AppLibrary.Domain.Entities;

namespace AppLibrary.Tests.TestEntities;

public class TestEntityWithNavigationProperties : IEntity
{
    public Guid Id { get; init; }
    public List<TextRecord> TextRecords { get; } = [];
}

public record TextRecord
{
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
}
