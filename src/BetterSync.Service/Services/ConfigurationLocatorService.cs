using BetterSync.Service.Core;

namespace BetterSync.Service.Services;

/// <summary>
/// The ConfigurationLocatorService is responsible for locating the configuration file for the application.
/// It searches for the configuration file in multiple locations based on the provided <see cref="ServiceOptions"/>.
/// If no file is found, it returns null.
/// </summary>
/// <param name="serviceOptions">The options containing configuration-related settings, including the file name.</param>
/// <exception cref="ArgumentNullException">Thrown if <paramref name="serviceOptions"/> is null.</exception>
internal sealed class ConfigurationLocatorService(ServiceOptions serviceOptions)
{
    private const string DefaultFolderName = "BetterSync";
    private const string DefaultConfigurationFileName = "BetterSync.config.json";
    
    private readonly ServiceOptions _serviceOptions =
        serviceOptions ?? throw new ArgumentNullException(nameof(serviceOptions), "ServiceOptions cannot be null");

    /// <summary>
    /// Asynchronously finds the configuration file based on the given options or default search locations.
    /// The method checks the following locations in this order:
    /// 1. If a specific configuration file path is provided via command-line arguments
    /// (in <see cref="ServiceOptions.ConfigFileName"/>).
    /// 2. In the local directory where the application is running.
    /// 3. In the user's AppData/Local directory under a specific folder ("BetterSync").
    /// 4. Directly in the user's home directory.
    /// If the file is found in any of these locations, the method returns the full path to the file.
    /// If no file is found, it returns null.
    /// </summary>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, 
    /// which contains the file path if found, or null if no configuration file is located.
    /// </returns>
    public ValueTask<string?> FindConfigurationAsync()
    {
        
        // Get filename from command line arguments
        if (!string.IsNullOrEmpty(_serviceOptions.ConfigFileName) && File.Exists(_serviceOptions.ConfigFileName))
        {
            return new ValueTask<string?>(_serviceOptions.ConfigFileName);
        }

        // Search in local directory
        var localFilePath = Path.Combine(Directory.GetCurrentDirectory(), DefaultConfigurationFileName);
        if (File.Exists(localFilePath))
        {
            return new ValueTask<string?>(localFilePath);
        }
        
        // Search in the user AppData-Directory 
        var userAppDataFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            DefaultFolderName,
            DefaultConfigurationFileName
        );
        if (File.Exists(userAppDataFilePath))
        {
            return new ValueTask<string?>(userAppDataFilePath);
        }
        
        // Search directly in the user directory
        var userHomeFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            DefaultConfigurationFileName
        );
        if (File.Exists(userHomeFilePath))
        {
            return new ValueTask<string?>(userHomeFilePath);
        }
        
        // If nothing was found return null (very sad)
        return new ValueTask<string?>(Task.FromResult<string?>(null));
    }
}
