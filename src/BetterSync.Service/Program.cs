using BetterSync.Service.Core;

namespace BetterSync.Service;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        SetupServiceLocator(host);    
        host.Run();
    }

    private static void SetupServiceLocator(IHost? host)
    {
        if (host == null)
            throw new InvalidOperationException("Application Host Context is null");    
        
        var serviceLocator = (ServiceLocator)ServiceLocator.Instance;
        serviceLocator.SetLocatorProvider(host.Services);
    }
}