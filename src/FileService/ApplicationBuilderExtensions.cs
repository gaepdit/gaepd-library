using GaEpd.FileService.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace GaEpd.FileService;

public static class ApplicationBuilderExtensions
{
    public static IServiceCollection AddFileServices(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new FileServiceSettings();
        configuration.GetSection(nameof(FileServiceSettings)).Bind(settings);

        switch (settings.FileService)
        {
            case FileServiceImplementation.InMemory:
                services.AddSingleton<IFileService, InMemory>();

                break;

            case FileServiceImplementation.FileSystem:
                if (string.IsNullOrWhiteSpace(settings.FileSystemBasePath))
                    throw new ConfigurationErrorsException(
                        "The File System Base Path must be supplied when using the File System implementation.");

                if (string.IsNullOrWhiteSpace(settings.NetworkUsername) &&
                    string.IsNullOrWhiteSpace(settings.NetworkDomain) &&
                    string.IsNullOrWhiteSpace(settings.NetworkPassword))
                {
                    services.AddTransient<IFileService, FileSystem>(_ => new FileSystem(settings.FileSystemBasePath));
                }
                else
                {
                    services.AddTransient<IFileService, FileSystem>(_ =>
                        new FileSystem(settings.FileSystemBasePath, settings.NetworkUsername, settings.NetworkDomain,
                            settings.NetworkPassword));
                }

                break;

            case FileServiceImplementation.AzureBlobStorage:
                if (string.IsNullOrEmpty(settings.AzureAccountName) || string.IsNullOrEmpty(settings.BlobContainer))
                    throw new ConfigurationErrorsException(
                        "The Azure account name and blob storage container name must be supplied when using the Azure Blob Storage implementation.");

                services.AddSingleton<IFileService, AzureBlobStorage>(_ =>
                    new AzureBlobStorage(settings.AzureAccountName, settings.BlobContainer, settings.BlobBasePath));

                break;

            default:
                throw new ConfigurationErrorsException(
                    "The File Service configuration must be set to 'InMemory', 'FileSystem', or 'AzureBlobStorage'.");
        }

        return services;
    }
}
