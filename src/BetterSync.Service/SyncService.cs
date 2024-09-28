using System.ServiceProcess;

namespace BetterSync.Service;

public sealed class SyncService : ServiceBase
{
    private IHost _host;
    
    public SyncService()
    {
        ServiceName = "BetterSync";
    }

    protected override void OnStart(string[] args)
    {
        _host = CreateHostBuilder(args).Build();
        _host.StartAsync();
    }

    protected override void OnPause()
    {
        base.OnPause();
    }
    
    protected override void OnStop()
    {
        base.OnStop();
    }

    protected override void OnShutdown()
    {
        base.OnShutdown();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            });
    }
}
