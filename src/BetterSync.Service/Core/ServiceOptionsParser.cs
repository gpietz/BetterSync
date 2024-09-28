using BetterSync.Common.Utilities;
using BetterSync.Service.Core.Exceptions;
using static BetterSync.Service.Core.ServiceOptionsParseError; 
    
namespace BetterSync.Service.Core;

/// <summary>
/// Parses command-line arguments and generates a <see cref="ServiceOptions"/> object.
/// </summary>
public class ServiceOptionsParser
{
    /// <summary>
    /// A delegate that handles the parsing of specific command-line arguments.
    /// </summary>
    /// <param name="options">The <see cref="ServiceOptions"/> object to update with the parsed data.</param>
    /// <param name="currentOption">The current command-line option being processed.</param>
    /// <param name="argsQueue">A queue of the remaining arguments to be processed.</param>
    private delegate void ArgumentHandler(ServiceOptions options, string currentOption, Queue<string> argsQueue);

    /// <summary>
    /// A dictionary that maps option strings to their corresponding argument handlers.
    /// </summary>
    private readonly Dictionary<string, ArgumentHandler> _options = new();

    /// <summary>
    /// Adds a new option and its corresponding argument handler to the parser.
    /// </summary>
    /// <param name="option">A string representing one or more options (delimited by '|').</param>
    /// <param name="argumentHandler">The handler that processes the option.</param>
    private void AddOption(string option, ArgumentHandler argumentHandler)
    {
        foreach (var opt in option.Split('|'))
        {
            _options[opt.Trim()] = argumentHandler;
        }
    }

    /// <summary>
    /// Finds the argument handler associated with the given option.
    /// </summary>
    /// <param name="option">The option string to search for.</param>
    /// <returns>The corresponding <see cref="ArgumentHandler"/> if found; otherwise, null.</returns>
    private ArgumentHandler? FindHandler(string option)  
        => _options.GetValueOrDefault(option.Trim());
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceOptionsParser"/> class, setting up the supported options.
    /// </summary>
    public ServiceOptionsParser()
    {
        AddOption("-f|--file", ParseFileArgument);
        AddOption("-v|--verbose", ParseVerboseArgument);
    }

    /// <summary>
    /// Parses the provided command-line arguments and returns a <see cref="Result{TSuccess, TError}"/> containing
    /// either a populated <see cref="ServiceOptions"/> or a <see cref="ServiceOptionsParseError"/>.
    /// </summary>
    /// <param name="args">The array of command-line arguments.</param>
    /// <returns>A result containing either a successful <see cref="ServiceOptions"/> or an error.</returns>
    public Result<ServiceOptions, ServiceOptionsParseError> Parse(string[] args)
    {
        var options = new ServiceOptions();
        var argsQueue = new Queue<string>(args);

        while (argsQueue.Count > 0)
        {
            try
            {
                var nextArg = argsQueue.Dequeue();
                var handler = FindHandler(nextArg);
                if (handler == null)
                {
                    return Result<ServiceOptions, ServiceOptionsParseError>.Err(
                        new(ErrorKind.InvalidArgument, $"Invalid or unknown argument: {nextArg}"));
                }

                handler(options, nextArg, argsQueue);
            }
            catch (ServiceArgumentException e)
            {
                return Result<ServiceOptions, ServiceOptionsParseError>.Err(new(e.ErrorKind, e.Message));
            }
            catch (Exception e)
            {
                return Result<ServiceOptions, ServiceOptionsParseError>.Err(new(ErrorKind.Unknown, e.Message));
            }
        }
        
        return Result<ServiceOptions, ServiceOptionsParseError>.Ok(options);
    }

    /// <summary>
    /// Parses the file argument from the command line and updates the <see cref="ServiceOptions"/> object.
    /// </summary>
    /// <param name="options">The <see cref="ServiceOptions"/> to update.</param>
    /// <param name="currentOption">The current command-line option being processed.</param>
    /// <param name="argsQueue">A queue of remaining arguments.</param>
    /// <exception cref="ServiceArgumentException">Thrown if a filename is missing or contains quotation errors.</exception>
    private static void ParseFileArgument(ServiceOptions options, string currentOption, Queue<string> argsQueue)
    {
        if (!argsQueue.TryDequeue(out var fileName))
            throw new ServiceArgumentException(ErrorKind.MissingArgument, $"Missing filename for option '{currentOption}'");

        fileName = fileName.Trim();

        if (fileName.StartsWith('\"') && !fileName.EndsWith("\""))
            throw new ServiceArgumentException(ErrorKind.ArgumentQuotationError,
                $"Quotation error for option '{currentOption}'; filename is beginning with quote but not ending!");
        if (!fileName.StartsWith("\"") && fileName.EndsWith("\""))
            throw new ServiceArgumentException(ErrorKind.ArgumentQuotationError,
                $"Quotation error for option '{currentOption}'; filename is ending with quote but not starting!");

        options.ConfigFileName = fileName;
    }

    /// <summary>
    /// Parses the verbose option from the command line and updates the <see cref="ServiceOptions"/> object.
    /// </summary>
    /// <param name="options">The <see cref="ServiceOptions"/> to update.</param>
    /// <param name="currentOption">The current command-line option being processed.</param>
    /// <param name="argsQueue">A queue of remaining arguments.</param>
    private static void ParseVerboseArgument(ServiceOptions options, string currentOptions, Queue<string> argsQueue)
    {
        options.Verbose = true;
    }
}
