namespace BetterSync.Service.BackgroundServices;

public abstract class BaseWorker<T>(ILogger<T> logger) : BackgroundService
{
    protected ILogger<T> Logger { get; } = logger;

    protected bool IsLogEnabled(LogLevel logLevel) => Logger.IsEnabled(logLevel);

    protected void Log(LogLevel logLevel, string? message, Exception? exception = null)
    {
        Logger.Log(logLevel, message, exception);
    }
}
