# Georgia EPD-IT File Service Library

This library was created by Georgia EPD-IT to provide common file services for our web applications.

## How to install

[![Nuget](https://img.shields.io/nuget/v/GaEpd.FileService)](https://www.nuget.org/packages/GaEpd.FileService)

To install, search for "GaEpd.FileService" in the NuGet package manager or run the following command:

`dotnet add package GaEpd.FileService`

## What's included

An `IFileService` interface is provided to abstract out common file persistence operations along with three useful
implementations:

### In Memory

```csharp
builder.Services.AddSingleton<IFileService, InMemoryFileService>();
```

### File System

The `basePath` is required and defines where the files will be stored.

```csharp
builder.Services.AddTransient<IFileService, FileSystemFileService>(_ =>
    new FileSystemFileService(basePath));
```

If a Windows Identity is required to access the desired file system, include the username, domain, and password in the
constructor.

```csharp
builder.Services.AddTransient<IFileService, FileSystemFileService>(_ =>
    new FileSystemFileService(basePath, userName, domain, password));
```

### Azure Blob Storage

Requires an Azure account and an existing Blob Storage container. (The Azure Blob service does not attempt to create the
container if it does not exist.) The `basePath` is optional and is added to file names as a path segment.

```csharp
builder.Services.AddSingleton<IFileService, AzureBlobFileService>(_ =>
    new AzureBlobFileService(accountName, container, basePath));
```

**Warning:** At the moment, the `SaveFileAsync()` method throws an exception when using Azure Blob Storage if the file
already exists. 
