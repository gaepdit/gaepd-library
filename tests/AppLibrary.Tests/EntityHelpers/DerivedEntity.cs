using GaEpd.AppLibrary.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AppLibrary.Tests.EntityHelpers;

public class DerivedEntity : IEntity
{
    public Guid Id { get; init; }

    [MaxLength(10)]
    public string Note { get; init; } = string.Empty;
}
