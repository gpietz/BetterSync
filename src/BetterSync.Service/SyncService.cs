using System.ServiceProcess;
using BetterSync.Service.BackgroundServices;
using BetterSync.Service.Core;

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
        _host = CreateHostBuilder(args).ConfigureServices((context, collection) =>
        {
            ServiceDistributor.Distribute(collection);
        }).Build();
       
        SetupServiceLocator(_host);
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
    
    private static void SetupServiceLocator(IHost? host)
    {
        if (host == null)
            throw new InvalidOperationException("Application Host Context is null");
    
        var serviceLocator = (ServiceLocator) ServiceLocator.Instance;
        serviceLocator.ResetLocatorProvider();
        serviceLocator.SetLocatorProvider(host.Services);
    }
}
