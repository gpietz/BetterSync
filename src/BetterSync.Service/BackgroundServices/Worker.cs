namespace BetterSync.Service.BackgroundServices;

public class Worker(ILogger<Worker> logger) : BaseWorker<Worker>(logger)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (IsLogEnabled(LogLevel.Information))
            {
                Log(LogLevel.Information, $"Worker running at: {DateTimeOffset.Now}");
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}
