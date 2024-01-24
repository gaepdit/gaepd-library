# Georgia EPD-IT App Library

This repo contains a library created by Georgia EPD-IT to provide common classes and tools for our web applications.

[![Georgia EPD-IT](https://raw.githubusercontent.com/gaepdit/gaepd-brand/main/blinkies/blinkies.cafe-gaepdit.gif)](https://github.com/gaepdit)
[![.NET Test](https://github.com/gaepdit/app-library/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/app-library/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/gaepdit/app-library/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/app-library/actions/workflows/codeql-analysis.yml)
[![SonarCloud Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_app-library&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_app-library)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_app-library&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_app-library)

(Much of this work was inspired by the [ABP Framework](https://abp.io/).)

## How to install

[![Nuget](https://img.shields.io/nuget/v/GaEpd.AppLibrary)](https://www.nuget.org/packages/GaEpd.AppLibrary)

To install , search for "GaEpd.AppLibrary" in the NuGet package manager or run the following command:

`dotnet add package GaEpd.AppLibrary`

## What's included

### Domain entities

The following interfaces and abstract implementations of domain entities are provided for domain driven design:

* The basic `IEntity<TKey>` interface defines an entity with a primary key of the given type.
* The special case `IEntity` interface defines an entity with a `GUID` primary key.
* `IAuditableEntity<TUserKey>` adds created/updated properties and methods for basic data auditing.
* `ISoftDelete` and `ISoftDelete<TUserKey>` add properties for "soft deleting" an entity rather than actually
* deleting it.
* `INamedEntity` adds a "Name" string property.
* `IActiveEntity` adds an "Active" boolean property.

There are also abstract classes based on the above interfaces from which you should derive your domain
entities: `Entity<TKey>`, `AuditableEntity<TKey, TUserKey>`, `SoftDeleteEntity<TKey, TUserKey>`,
`AuditableSoftDeleteEntity<TKey, TUserKey>`, and `StandardNamedEntity`.

The `StandardNamedEntity` class derives from `AuditableEntity<Guid>`, `INamedEntity`, and `IActiveEntity`, and includes
methods for enforcing the length of the `Name`. Maximum and minimum name length can be set in the constructor.

Example usage:

```csharp
public class DerivedNamedEntity : StandardNamedEntity
{
    public override int MinNameLength => 2;
    public override int MaxNameLength => 50;

    public DerivedNamedEntity() { }
    public DerivedNamedEntity(Guid id, string name) : base(id, name) { }
}
```

### ValueObject

An abstract ValueObject record can help add value objects to your domain entities.
A [value object](https://www.martinfowler.com/bliki/ValueObject.html) is a compound of properties, such as an address or
date range, that are comparable based solely on their values rather than their references. The properties of a value
object are typically stored with its parent class, not as a separate record with its own ID. Value objects should be
treated as immutable. When deriving from `ValueObject`, you must provide a `GetEqualityComponents()` method to define
which properties to use to determine equality.

Example usage:

```csharp
[Owned]
public record Address : ValueObject
{
    public string Street { get; init; } = string.Empty;
    public string? Street2 { get; init; }
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;

    [DataType(DataType.PostalCode)]
    public string PostalCode { get; init; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return Street2 ?? string.Empty;
        yield return City;
        yield return State;
        yield return PostalCode;
    }
}
```

Note: The `[Owned]` attribute is an Entity Framework attribute defining this as a value object owned by the parent
class. See [Owned Entity Types](https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities) for more info on how
this is implemented in EF Core.

### Data Repositories

Common data repository interfaces define basic entity CRUD operations. The `IReadRepository` interface defines get and
search operations (including paginated search). The `IWriteRepository` interface defines insert, update, and delete
operations. `IRepository` combines the read and write interfaces. Finally, the `INamedEntityRepository` adds a
find-by-name method.

(Note that these interfaces work directly with domain entities. Your application should
define [application/domain services](https://docs.abp.io/en/abp/latest/Domain-Services#application-services-vs-domain-services)
that define how the application interacts with the entities & repositories through data transfer objects (DTOs).)

There are two abstract `BaseRepository` classes that each implement the `IRepository` interface, one using in-memory
data and the other requiring an Entity Framework database context.

There are similarly two abstract `NamedEntityRepository` classes that each implement the `INamedEntityRepository`
interface.

Example usage:

```csharp
public interface IDerivedRepository : INamedEntityRepository<DerivedNamedEntity> { }

public sealed class DerivedRepository : NamedEntityRepository<DerivedNamedEntity, AppDbContext>, IDerivedRepository
{
    public DerivedRepository(AppDbContext context) : base(context) { }
}

```

### Predicate builder

Code from [C# in a Nutshell](https://www.albahari.com/nutshell/predicatebuilder.aspx) is included to enable creating
filter expressions that can be combined. The library comes with the commonly used filters `WithId(id)` for all entities
and `ExcludedDeleted()` for "soft delete" entities.

Example usage:

```csharp
public static Expression<Func<MyEntity, bool>> IsActive(this Expression<Func<MyEntity, bool>> predicate) =>
    predicate.And(e => e.IsActive);

public static Expression<Func<MyEntity, bool>> ActiveAndNotDeletedPredicate() =>
    PredicateBuilder.True<MyEntity>().IsActive().ExcludeDeleted();
```

### Pagination classes

`IPaginatedRequest` and `IPaginatedResult<T>` define how to request and receive paginated (and sorted) search results.

The [System.Linq.Dynamic.Core](https://github.com/zzzprojects/System.Linq.Dynamic.Core) package is included.

### List Item record

A `ListItem<TKey>` record type defines a key-value pair with fields for ID of type `TKey` and `string` Name.
The `ToSelectList()` extension method takes a `ListItem` enumerable and returns an MVC `SelectList` which can be used to
create an HTML `<select>` element.

### Enum extensions

`GetDisplayName()` and `GetDescription()` return the `DisplayAttribute.Name` and `DescriptionAttribute` values of an
enum, respectively.

### Guard clauses

The [GuardClauses](https://github.com/gaepdit/guard-clauses) package is included by reference.

## What's not included

The [File Service](https://github.com/gaepdit/file-service) and [GuardClauses](https://github.com/gaepdit/guard-clauses)
packages have been moved to separate repositories.
