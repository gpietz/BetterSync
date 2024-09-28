using System.Text.Json;
using System.Text.Json.Serialization;
using BetterSync.Common.Configuration;
using BetterSync.Common.Utilities;

namespace BetterSync.Common.Services;

public class ConfigurationService
{
    /// <summary>
    /// Asynchronously loads the synchronization configuration from a specified file path.
    /// </summary>
    /// <param name="filePath">The path to the configuration file. Must not be null, empty, or whitespace.</param>
    /// <returns>A <see cref="Result{SyncConfiguration, SyncConfigurationError}"/> containing the configuration if
    /// successful, or an error if the file path is invalid, the file does not exist, or the loading process fails.</returns>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Validates that the provided file path is not null, empty, or whitespace.
    /// 2. Checks whether the file exists at the provided file path.
    /// 3. Attempts to read and deserialize the file into a <see cref="SyncConfiguration"/> object using JSON.
    /// If any of these steps fail, the appropriate error result is returned.
    /// </remarks>
    /// <exception cref="SyncConfigurationError">
    /// Thrown when the file path is invalid, the file does not exist, or there is an issue during loading or deserialization.
    /// </exception>
    public async Task<Result<SyncConfiguration, SyncConfigurationError>> LoadConfigurationAsync(string filePath)
    {
        // Check if file path is null or empty
        if (string.IsNullOrWhiteSpace(filePath))
            return Result<SyncConfiguration, SyncConfigurationError>.Err(
                new SyncConfigurationError(SyncConfigurationError.ErrorKind.InvalidFileName,
                    "Filename is null or empty"));

        // Check if file exists
        if (!File.Exists(filePath))
            return Result<SyncConfiguration, SyncConfigurationError>.Err(
                new SyncConfigurationError(SyncConfigurationError.ErrorKind.FileNotFound,
                    $"File not found: {filePath}"));

        try
        {
            // Load and deserialize JSON
            var json = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
            var syncConfiguration = JsonSerializer.Deserialize<SyncConfiguration>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            // Ensure deserialization was successful
            if (syncConfiguration == null)
            {
                return Result<SyncConfiguration, SyncConfigurationError>.Err(
                    new(SyncConfigurationError.ErrorKind.FileLoadError,
                        $"Configuration is null: {filePath}"));
            }

            return Result<SyncConfiguration, SyncConfigurationError>.Ok(syncConfiguration);
        }
        catch (UnauthorizedAccessException e)
        {
            // Handle unauthorized access separately
            return Result<SyncConfiguration, SyncConfigurationError>.Err(
                new(SyncConfigurationError.ErrorKind.AccessDenied,
                    $"Access denied for configuration file: {filePath}", e));
        }
        catch (IOException e) when (e.HResult == unchecked((int)0x80070020) || // FILE_SHARE_VIOLATION
                                    e.HResult == unchecked((int)0x80070005)) // ACCESS_DENIED
        {
            // Specific handling for file access issues
            return Result<SyncConfiguration, SyncConfigurationError>.Err(
                new(SyncConfigurationError.ErrorKind.AccessDenied,
                    $"Access denied or file in use: {filePath}", e));
        }
        catch (IOException e)
        {
            // General IOException handling
            return Result<SyncConfiguration, SyncConfigurationError>.Err(
                new(SyncConfigurationError.ErrorKind.FileLoadError,
                    $"Failed to load configuration file: {filePath}", e));
        }
        catch (JsonException e)
        {
            // Handle deserialization errors
            return Result<SyncConfiguration, SyncConfigurationError>.Err(
                new(SyncConfigurationError.ErrorKind.DeserializeError,
                    $"Failed to deserialize configuration file: {filePath}", e));
        }
        catch (Exception e)
        {
            // Catch-all for any other exceptions
            return Result<SyncConfiguration, SyncConfigurationError>.Err(
                new(SyncConfigurationError.ErrorKind.FileLoadError,
                    $"Unexpected error loading configuration file: {filePath}", e));
        }
    }
}
