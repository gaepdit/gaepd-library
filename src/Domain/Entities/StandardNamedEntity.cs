using GaEpd.AppLibrary.GuardClauses;
using System.Text;

namespace GaEpd.AppLibrary.Domain.Entities;

public abstract class StandardNamedEntity : AuditableEntity, INamedEntity, IActiveEntity
{
    // Entity properties

    private static int _minNameLength = 1;
    private static int _maxNameLength;

    /// <summary>
    /// The minimum allowable length for the <see cref="Name"/>. Must be greater than zero.
    /// </summary>
    public static int MinNameLength
    {
        get => _minNameLength;
        protected set => _minNameLength = Guard.Positive(value);
    }

    /// <summary>
    /// The maximum allowable length for the <see cref="Name"/>. Must be equal to or greater
    /// than <see cref="MinNameLength"/>, unless set to zero. When set to zero, no maximum
    /// length is enforced.
    /// </summary>
    public static int MaxNameLength
    {
        get => _maxNameLength;
        protected set
        {
            if (value == 0) _maxNameLength = 0;
            if (value < _minNameLength)
                throw new ArgumentException("Maximum length must be greater than or equal to minimum.", nameof(value));
            _maxNameLength = value;
        }
    }

    // Constructors

    protected StandardNamedEntity() { }

    protected StandardNamedEntity(Guid id, string name) : base(id) => SetName(name);

    // Properties

    public string Name { get; private set; } = string.Empty;
    public bool Active { get; set; } = true;

    // Methods

    internal void ChangeName(string name) => SetName(name);

    private void SetName(string name) =>
        Name = Guard.ValidLength(name.Trim(), minLength: MinNameLength,
            maxLength: MaxNameLength > 0 ? MaxNameLength : int.MaxValue);

    // Display properties

    public string NameWithActivity
    {
        get
        {
            var sn = new StringBuilder();
            sn.Append(Name);
            if (!Active) sn.Append(" [Inactive]");
            return sn.ToString();
        }
    }
}
