# Changelog

## [Unreleased]

### Added

- Added a method to list files in a specified path.

### Changed

- **Breaking change** This release changes how paths are built when using Azure Blob Storage in order to avoid platform
  inconsistencies. It's possible this could result in `FileExistsAsync` and `GetFilesAsync` failing for existing files.
  Thorough testing is recommended when updating.

## [2.1.0]

### Added

- Added an extension method for registering and configuring file services.

## [2.0.0]

- Upgraded to .NET 8.0.

## [1.0.0]

- Initial release.
