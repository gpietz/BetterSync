namespace BetterSync.Service.Core.Exceptions;

/// <summary>
/// Represents an exception that is thrown when there is an issue with service arguments.
/// </summary>
/// <param name="errorKind">The type of error.</param>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="argument">The argument that caused the exception, if available.</param>
public class ServiceArgumentException(
    ServiceOptionsParseError.ErrorKind errorKind,
    string message,
    string? argument = null) : Exception(message)
{
    /// <summary>
    /// Gets the argument that caused the exception, if any.
    /// </summary>
    public string? Argument { get; } = argument;

    public ServiceOptionsParseError.ErrorKind ErrorKind { get; } = errorKind;
}
