using BetterSync.Service.Services;

namespace BetterSync.Service.Core;

/// <summary>
/// The ServiceDistributor class provides a centralized way to register services into the application's 
/// dependency injection container. It simplifies the addition of new services by encapsulating the registration logic.
/// </summary>
internal static class ServiceDistributor
{
    /// <summary>
    /// Registers necessary services to the application's <see cref="IServiceCollection"/>.
    /// This method is responsible for adding all services needed by the application,
    /// such as the <see cref="ConfigurationLocatorService"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> instance where the services
    /// are to be registered.</param>
    public static void Distribute(IServiceCollection services)
    {
        services.AddScoped<ConfigurationLocatorService>();
    }
}
