using BetterSync.Service.Services;

namespace BetterSync.Service.BackgroundServices;

public class ConfigurationMonitorWorker(ILogger<ConfigurationMonitorWorker> logger) 
    : BaseWorker<ConfigurationMonitorWorker>(logger)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}
