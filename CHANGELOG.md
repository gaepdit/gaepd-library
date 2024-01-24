# Changelog

# [5.1.0]

- Updated the included GuardClauses library to v2.0.0.

# [5.0.1]

- Updated changelog for v5.0.0 release.

## [5.0.0]

- Upgraded to .NET 8.0.

### Added

- Added entity and repository interfaces that default to using a GUID primary key and updated the abstract classes to use these new interfaces.

### Changed

- **Breaking changes:**
  - Uses of `EntityNotFoundException` will need to be updated to provide the class type. For example, `EntityNotFoundException(typeof(MyEntity), id)` should be replaced with `EntityNotFoundException<MyEntity>(id)`.
  - References to `IEntity<Guid>` may need to be replaced with `IEntity`.

## [4.1.0]

- Implement IAsyncDisposable in repositories.

## [4.0.0]

- Move GuardClauses to a separate NuGet package.

## [3.5.1]

- Derived EF repositories can now specify the DbContext type.

## [3.5.0]

- Added an abstract StandardNameEntity along with INamedEntity and IActiveEntity interfaces.
- Added INamedEntityRepository and INamedEntityManager interfaces and implementations.

## [3.4.0]

- Included abstract implementations of BaseRepository.

## [3.3.0]

- Added a "ConcatWithSeparator" string extension.
- Added "PreviousPageNumber" and "NextPageNumber" properties to the IPaginatedResult interface.
- Made some possible performance improvements to the Enum extensions. 
  Breaking change: The Enum extensions no longer work with nullable Enum values. 

## [3.2.0]

- Added a "SetNotDeleted" (undelete) method to the ISoftDelete interface.

## [3.1.0]

- Added a "SaveChanges" method to the write repository.

## [3.0.0]

- Upgraded the library to .NET 7.

## [2.0.0]

- Moved the write repository operations to a separate interface.
- Added "Exists" methods to the read repository interface.
- Renamed the user ID properties on auditable entities.

## [1.1.0]

- Added predicate builder and common entity filters.
- Added enum extensions.
- Added the System.Linq.Dynamic.Core package.

## [1.0.1]

- Added a Readme file to the package.

## [1.0.0]

- Initial release.
