using BetterSync.Common.Core;

namespace BetterSync.Common.Configuration;

/// <summary>
/// Represents an error that occurs during the synchronization configuration process.
/// </summary>
/// <param name="kind">The type of error.</param>
/// <param name="message">The message that describes the error.</param>
/// <param name="innerException">The optional inner exception that caused the current error.</param>
public class SyncConfigurationError(
    SyncConfigurationError.ErrorKind kind,
    string message,
    Exception? innerException = null)
    : ErrorResult<SyncConfigurationError.ErrorKind>(kind, message, innerException)
{
    /// <summary>
    /// Defines the different types of errors that can occur during synchronization configuration.
    /// </summary>
    public enum ErrorKind
    {
        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// The file specified for synchronization was not found.
        /// </summary>
        FileNotFound = 1,
        
        /// <summary>
        /// Access to the configuration file was denied.
        /// </summary>
        AccessDenied = 2,
        
        /// <summary>
        /// The configuration file contains invalid or malformed data.
        /// </summary>
        InvalidConfiguration = 3,
        
        /// <summary>
        /// The file name provided is invalid.
        /// </summary>
        InvalidFileName = 4,
        
        /// <summary>
        /// An error occurred while deserializing the configuration file.
        /// </summary>
        DeserializeError = 5,
        
        /// <summary>
        /// An error occurred while loading the configuration file.
        /// </summary>
        FileLoadError = 6,
    }
}
