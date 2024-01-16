# Georgia EPD-IT File Service Library

This library was created by Georgia EPD-IT to provide common file services for our web applications.

## Installation

[![Nuget](https://img.shields.io/nuget/v/GaEpd.FileService)](https://www.nuget.org/packages/GaEpd.FileService)

To install, search for "GaEpd.FileService" in the NuGet package manager or run the following command:

`dotnet add package GaEpd.FileService`

## Usage

An `IFileService` interface is used to abstract out common file persistence operations:

* `SaveFileAsync`
* `FileExistsAsync`
* `GetFileAsync`
* `TryGetFileAsync`
* `DeleteFileAsync`

The library also includes three useful implementations, In Memory, File System, and Azure Blob Storage. Each
implementation can be registered independently as shown in the sections below.

Alternatively, the `AddFileServices` extension method can be used to automatically register an implementation based on
the configuration. To do so, register the file services configuration as follows:

```csharp
builder.Services.AddFileServices(builder.Configuration);
```

And add the following section to your configuration:

```json
{
  "FileServiceSettings": {
    "FileService": "",
    "FileSystemBasePath": "",
    "NetworkUsername": "",
    "NetworkDomain": "",
    "NetworkPassword": "",
    "AzureAccountName": "",
    "BlobContainer": "",
    "BlobBasePath": ""
  }
}
```

The `FileService` setting must be set to "InMemory", "FileSystem", or "AzureBlobStorage".

* If `InMemory` is chosen, all other settings are ignored.

* If `FileSystem` is chosen, then `FileSystemBasePath` is required, and `NetworkUsername`, `NetworkDomain`,
  and `NetworkPassword` can be provided if needed. Other settings are ignored.

* If `AzureBlobStorage` is chosen, then `AzureAccountName` and `BlobContainer` are required, and `BlobBasePath` can be
  provided if desired. Other settings are ignored.

### In Memory

The in-memory file service implementation stores files in memory.

```csharp
builder.Services.AddSingleton<IFileService, InMemoryFileService>();
```

### File System

The file system service writes files to a local or network drive. The `basePath` parameter is required and defines where
the files will be stored.

```csharp
builder.Services.AddTransient<IFileService, FileSystemFileService>(_ =>
    new FileSystemFileService(basePath));
```

If a Windows Identity is required to access the desired file location, use the overload that
accepts `username`, `domain`, and `password` parameters in the constructor.

```csharp
builder.Services.AddTransient<IFileService, FileSystemFileService>(_ =>
    new FileSystemFileService(basePath, username, domain, password));
```

### Azure Blob Storage

The Azure Blob Storage service requires an Azure account and an existing Blob Storage container. (The service does not
attempt to create the container if it does not exist.) The `basePath` parameter is optional and is prepended to file
names as a path segment.
[`DefaultAzureCredential`](https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication/?tabs=command-line#defaultazurecredential)
is used to initialize the `BlobServiceClient`.

```csharp
builder.Services.AddSingleton<IFileService, AzureBlobFileService>(_ =>
    new AzureBlobFileService(accountName, container, basePath));
```

**Warning:** At the moment, the `SaveFileAsync()`
method [throws an exception](https://github.com/Azure/azure-sdk-for-net/issues/39473) when using Azure Blob Storage if
the file already exists. 
