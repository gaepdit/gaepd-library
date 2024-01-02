using GaEpd.AppLibrary.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AppLibrary.Tests.EntityHelpers;

public class DerivedEntity : IEntity<Guid>
{
    public Guid Id { get; init; }

    [MaxLength(10)]
    public string Name { get; init; } = string.Empty;
}
