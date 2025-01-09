using GaEpd.AppLibrary.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AppLibrary.Tests.TestEntities;

public class TestEntityWithNavigationProperties : IEntity
{
    public Guid Id { get; init; }
    [StringLength(7)] public string Name { get; init; } = string.Empty;
    public List<TextRecord> TextRecords { get; } = [];
}

public record TextRecord
{
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
}
