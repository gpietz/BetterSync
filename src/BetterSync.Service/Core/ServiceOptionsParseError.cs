using BetterSync.Common.Core;

namespace BetterSync.Service.Core;

/// <summary>
/// Represents an error that occurs while parsing service options.
/// </summary>
/// <remarks>
/// This class extends the generic <see cref="ErrorResult{TErrorKind}"/> class, with the error kind
/// being specific to service option parsing.
/// </remarks>
/// <param name="kind">The kind of error that occurred.</param>
/// <param name="message">A message that describes the error.</param>
/// <param name="innerException">An optional inner exception that caused the current error.</param>
public class ServiceOptionsParseError(
    ServiceOptionsParseError.ErrorKind kind,
    string message,
    Exception? innerException = null)
    : ErrorResult<ServiceOptionsParseError.ErrorKind>(kind, message, innerException)
{
    /// <summary>
    /// Defines the different kinds of errors that can occur while parsing service options.
    /// </summary>
    public enum ErrorKind
    {
        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        Unknown,
        
        /// <summary>
        /// An invalid argument was provided.
        /// </summary>
        InvalidArgument,

        /// <summary>
        /// An argument is missing for an option.
        /// </summary>
        MissingArgument,
        
        /// <summary>
        /// An error occurred due to incorrect quotation in arguments.
        /// </summary>
        ArgumentQuotationError,
        
    }
}
