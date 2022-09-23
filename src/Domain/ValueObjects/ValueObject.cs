namespace GaEpd.Library.Domain.ValueObjects;

/// <summary>
/// Creates a Value Object record.
/// </summary>
public abstract record ValueObject
{
    /// <summary>
    /// This method defines which properties of the Value Object to use to determine equality.
    /// </summary>
    /// <returns>An enumeration of properties to compare to determine value object equality.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override int GetHashCode() =>
        GetEqualityComponents()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
}
