using GaEpd.AppLibrary.GuardClauses;
using System.Text;

namespace GaEpd.AppLibrary.Domain.Entities;

/// <summary>
/// An <see cref="AuditableEntity"/> that also implements <see cref="INamedEntity"/>
/// and <see cref="IActiveEntity"/>.
/// </summary>
public abstract class StandardNamedEntity : AuditableEntity, INamedEntity, IActiveEntity
{
    // Entity properties

    /// <summary>
    /// The minimum allowable length for the <see cref="Name"/>. Must be greater than zero.
    /// </summary>
    public virtual int MinNameLength => 1;

    /// <summary>
    /// The maximum allowable length for the <see cref="Name"/>. Must be equal to or greater
    /// than <see cref="MinNameLength"/>, unless set to zero. When set to zero, no maximum
    /// length is enforced.
    /// </summary>
    public virtual int MaxNameLength => 0;

    // Constructors

    protected StandardNamedEntity() { }
    protected StandardNamedEntity(Guid id, string name) : base(id) => SetName(name);

    // Properties

    public string Name { get; private set; } = string.Empty;
    public bool Active { get; set; } = true;

    // Methods
    internal void SetId(Guid id) => Id = id;

    internal void SetName(string name) => Name = Guard.ValidLength(name.Trim(), minLength: MinNameLength,
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
