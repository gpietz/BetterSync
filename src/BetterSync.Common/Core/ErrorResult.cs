namespace BetterSync.Common.Core;

/// <summary>
/// Represents a generic error that occurs during the synchronization configuration process.
/// </summary>
/// <typeparam name="TErrorKind">The type that represents the kind of error.</typeparam>
public class ErrorResult<TErrorKind>
{
    /// <summary>
    /// Gets the type of the error.
    /// </summary>
    public TErrorKind Kind { get; }
    
    /// <summary>
    /// Gets the message that describes the error.
    /// </summary>
    public string Message { get; }
    
    /// <summary>
    /// Gets the inner exception that caused the current error, if available.
    /// </summary>
    public Exception? InnerException { get; }

    /// <summary>
    /// Initializes a new instance of this class with the specified error kind, message, and optional inner exception.
    /// </summary>
    /// <param name="kind">The type of error.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The optional inner exception that caused the current error.</param>
    public ErrorResult(TErrorKind kind, string message, Exception? innerException = null)
    {
        Kind = kind;
        Message = message;
        InnerException = innerException;
    }
    
    /// <summary>
    /// Returns a string that represents the current error.
    /// </summary>
    /// <returns>A string containing the error kind, message, and inner exception (if available).</returns>
    public override string ToString()
    {
        return $"{Kind}: {Message}{(InnerException != null ? InnerException.Message : string.Empty)}";
    }
}
