# Georgia EPD-IT App Library

This library was created by Georgia EPD-IT to provide common classes and tools for our web applications. (Much of this work was inspired by the [ABP Framework](https://abp.io/).)

[![.NET Test](https://github.com/gaepdit/app-library/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gaepdit/app-library/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/gaepdit/app-library/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/gaepdit/app-library/actions/workflows/codeql-analysis.yml)
[![SonarCloud Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_app-library&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=gaepdit_app-library)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=gaepdit_app-library&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=gaepdit_app-library)

## How to install

To install , search for "GaEpd.AppLibrary" in the NuGet package manager or run the following command:

`dotnet add package GaEpd.AppLibrary`

## What's included

### Guard clauses

Guard clauses simplify checking for invalid input parameters. (This might be moved to a separate package later or replaced with a third-party tool like [ardalis/GuardClauses](https://github.com/ardalis/GuardClauses).)

Example usage:

```csharp
public class SomeClass
{
    private readonly string _name;

    public SomeClass(string name)
    {
        _name = Guard.NotNullOrWhiteSpace(name);
    }
}
```

### Domain entities

The following interfaces and abstract implementations of domain entities are provided for domain driven design:

* The basic `IEntity<TKey>` interface defines a basic entity with a primary key of the given type.
* `IAuditableEntitey<TUserKey>` adds creation/update properties and methods for basic data auditing.
* `ISoftDelete<TUserKey>` adds properties for "soft deleting" an entity rather than actually deleting it.

There are also abstract classes based on the above that you can derive your domain entities from, including `Entity<TKey>`, `AuditableEntity<TKey, TUserKey>`, `SoftDeleteEntity<TKey, TUserKey>`, and `AuditableSoftDeleteEntity<TKey, TUserKey>`.

### ValueObject

An abstract ValueObject record can help add value objects to your domain entities. A [value object](https://www.martinfowler.com/bliki/ValueObject.html) is a compound of properties, such as an address or date range, that are comparable based solely on their values rather than their references. The properties of a value object are typically stored with its parent class, not as a separate record with its own ID. Value objects should be treated as immutable. When deriving from `ValueObject`, you will have to provide a `GetEqualityComponents()` method to define which properties to use to determine equality.

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

Note: The `[Owned]` attribute is an Entity Framework attribute defining this as a value object owned by the parent class. See [Owned Entity Types](https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities) for more info on how this is implemented in EF Core.

### Repository interfaces

Common repository interfaces define basic entity CRUD operations. The `IReadOnlyRepository<TEntity, in TKey>` interface defines get and search operations (including paginated search). `IRepository<TEntity, in TKey>` adds write operations.

Note that these interfaces work directly with domain entities. Your application should define [application/domain services](https://docs.abp.io/en/abp/latest/Domain-Services#application-services-vs-domain-services) that define how the application interacts with the entities & repositories through data transfer objects (DTOs).  

### Pagination classes

`IPaginatedRequest` and `IPaginatedResult<T>` define how to request and receive paginated (and sorted) search results. 

### List Item record

A `ListItem<TKey>` record type defines a key-value pair with fields for ID of type `TKey` and `string` Name. The `ToSelectList()` extension method takes a `ListItem` enumerable and returns an MVC `SelectList` which can be used to create an HTML `<select>` element.
