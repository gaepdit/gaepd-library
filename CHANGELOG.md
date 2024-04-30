# Changelog

## [5.2.1] - 2024-04-30

- Added a `GetOrderedListAsync` overload method with predicate matching.

## [5.2.0] - 2024-04-30

- Added a string `Truncate` function.
- Added a `GetOrderedListAsync` method to the Named Entity Repository.

## [5.1.0] - 2024-01-03

- Updated the included GuardClauses library to v2.0.0.

## [5.0.1] - 2024-01-02

- Updated changelog for v5.0.0 release.

## [5.0.0] - 2024-01-02

- Upgraded to .NET 8.0.

### Added

- Added entity and repository interfaces that default to using a GUID primary key and updated the abstract classes to use these new interfaces.

### Changed

- **Breaking changes:**
  - Uses of `EntityNotFoundException` will need to be updated to provide the class type. For example, `EntityNotFoundException(typeof(MyEntity), id)` should be replaced with `EntityNotFoundException<MyEntity>(id)`.
  - References to `IEntity<Guid>` may need to be replaced with `IEntity`.

## [4.1.0] - 2023-11-09

- Implement IAsyncDisposable in repositories.

## [4.0.0] - 2023-10-25

- Move GuardClauses to a separate NuGet package.

## [3.5.1] - 2023-09-20

- Derived EF repositories can now specify the DbContext type.

## [3.5.0] - 2023-09-19

- Added an abstract StandardNameEntity along with INamedEntity and IActiveEntity interfaces.
- Added INamedEntityRepository and INamedEntityManager interfaces and implementations.

## [3.4.0] - 2023-09-18

- Included abstract implementations of BaseRepository.

## [3.3.0] - 2023-08-11

- Added a "ConcatWithSeparator" string extension.
- Added "PreviousPageNumber" and "NextPageNumber" properties to the IPaginatedResult interface.
- Made some possible performance improvements to the Enum extensions. 
  Breaking change: The Enum extensions no longer work with nullable Enum values. 

## [3.2.0] - 2023-05-22

- Added a "SetNotDeleted" (undelete) method to the ISoftDelete interface.

## [3.1.0] - 2023-04-25

- Added a "SaveChanges" method to the write repository.

## [3.0.0] - 2023-04-25

- Upgraded the library to .NET 7.

## [2.0.0] - 2023-03-07

- Moved the write repository operations to a separate interface.
- Added "Exists" methods to the read repository interface.
- Renamed the user ID properties on auditable entities.

## [1.1.0] - 2023-03-07

- Added predicate builder and common entity filters.
- Added enum extensions.
- Added the System.Linq.Dynamic.Core package.

## [1.0.1] - 2022-10-14

- Added a Readme file to the package.

## [1.0.0] - 2022-10-06

_Initial release._

[5.2.1]: https://github.com/gaepdit/app-library/releases/tag/v5.2.1
[5.2.0]: https://github.com/gaepdit/app-library/releases/tag/v5.2.0
[5.1.0]: https://github.com/gaepdit/app-library/releases/tag/l%2Fv5.1.0
[5.0.1]: https://github.com/gaepdit/app-library/releases/tag/al%2Fv5.0.1
[5.0.0]: https://github.com/gaepdit/app-library/releases/tag/al%2Fv5.0.0
[4.1.0]: https://github.com/gaepdit/app-library/releases/tag/al%2Fv4.1.0
[4.0.0]: https://github.com/gaepdit/app-library/releases/tag/al%2Fv4.0.0
[3.5.1]: https://github.com/gaepdit/app-library/releases/tag/v3.5.1
[3.5.0]: https://github.com/gaepdit/app-library/releases/tag/v3.5.0
[3.4.0]: https://github.com/gaepdit/app-library/releases/tag/v3.4.0
[3.3.0]: https://github.com/gaepdit/app-library/releases/tag/v3.3.0
[3.2.0]: https://github.com/gaepdit/app-library/releases/tag/v3.2.0
[3.1.0]: https://github.com/gaepdit/app-library/releases/tag/v3.1.0
[3.0.0]: https://github.com/gaepdit/app-library/releases/tag/v3.0.0
[2.0.0]: https://github.com/gaepdit/app-library/releases/tag/v2.0.0
[1.1.0]: https://github.com/gaepdit/app-library/releases/tag/v1.1.0
[1.0.1]: https://github.com/gaepdit/app-library/releases/tag/v1.0.1
[1.0.0]: https://github.com/gaepdit/app-library/releases/tag/v1.0.0
