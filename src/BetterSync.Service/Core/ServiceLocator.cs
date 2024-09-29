namespace BetterSync.Service.Core;

/// <summary>
/// Interface for accessing services in a Dependency Injection (DI) container.
/// Provides methods to retrieve services, with or without requiring non-null values.
/// </summary>
public interface IServiceLocator 
{
    /// <summary>
    /// Retrieves a service of the specified type T from the DI container.
    /// Returns null if the service is not found.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <returns>The service instance if found, otherwise null.</returns>
    public T? GetService<T>();
    
    /// <summary>
    /// Retrieves a service of the specified type T from the DI container.
    /// Throws an exception if the service is not found.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve. Must be a non-nullable type.</typeparam>
    /// <returns>The service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the service is not found or the
    /// ServiceProvider is not initialized.</exception>
    public T GetRequiredService<T>() where T : notnull;
}

/// <summary>
/// Singleton implementation of the IServiceLocator interface.
/// Manages the retrieval of services from the DI container and ensures only one instance of the locator exists.
/// </summary>
internal class ServiceLocator : IServiceLocator
{
    /// <summary>
    /// Lazy-loaded singleton instance of the <see cref="IServiceLocator"/>.
    /// Ensures that the instance is created only when it is first accessed.
    /// </summary>
    private static readonly Lazy<IServiceLocator> ClassInstance = new(() => new ServiceLocator()); 
    
    /// <summary>
    /// Provides access to the singleton instance of ServiceLocator.
    /// </summary>
    public static IServiceLocator Instance => ClassInstance.Value;

    /// <summary>
    /// Private constructor to prevent external instantiation.
    /// </summary>
    private ServiceLocator()
    {
    }
    
    /// <summary>
    /// Backing field for the IServiceProvider used to resolve services.
    /// </summary>
    private static IServiceProvider? _serviceProvider;

    private readonly object _setLocatorProviderLock = new();

    /// <summary>
    /// Initializes the ServiceLocator with the specified IServiceProvider.
    /// This method should be called only once to ensure that the ServiceProvider is set.
    /// </summary>
    /// <param name="serviceProvider">The IServiceProvider instance to use for service resolution.</param>
    /// <exception cref="InvalidOperationException">Thrown if the ServiceProvider is already initialized.</exception>
    internal void SetLocatorProvider(IServiceProvider serviceProvider)
    {
        EnsureServiceProviderNotSet();
        
        lock (_setLocatorProviderLock)
        {
            EnsureServiceProviderNotSet();    
            _serviceProvider = serviceProvider;    
        }
        
        return;
        
        void EnsureServiceProviderNotSet()
        {
            if (_serviceProvider != null)
            {
                throw new InvalidOperationException("ServiceProvider is already initialized.");
            }
        }
    }

    /// <summary>
    /// Resets the current IServiceProvider in the ServiceLocator.
    /// This method allows the ServiceLocator to be re-initialized with a new IServiceProvider.
    /// Should be called when a new Host is created to ensure the ServiceLocator is updated with the correct provider.
    /// </summary>
    /// <remarks>
    /// This method is useful in scenarios where the application host may be restarted or recreated, 
    /// such as in background services or worker roles. It ensures that the ServiceLocator 
    /// does not hold onto outdated or invalid IServiceProvider instances.
    /// </remarks>
    internal void ResetLocatorProvider()
    {
        lock (_setLocatorProviderLock)
        {
            _serviceProvider = null;
        }
    }
        
    /// <summary>
    /// Retrieves a service of the specified type T from the DI container.
    /// Returns null if the service is not found.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve.</typeparam>
    /// <returns>The service instance if found, otherwise null.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the ServiceProvider is not initialized.</exception>
    public T? GetService<T>()
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("ServiceProvider has not been initialized.");
        }

        return _serviceProvider.GetService<T>();
    }

    /// <summary>
    /// Retrieves a service of the specified type T from the DI container.
    /// Throws an exception if the service is not found.
    /// </summary>
    /// <typeparam name="T">The type of the service to retrieve. Must be a non-nullable type.</typeparam>
    /// <returns>The service instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the ServiceProvider is not initialized or the
    /// service cannot be found.</exception>
    public T GetRequiredService<T>() where T : notnull
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("ServiceProvider has not been initialized.");
        }
        
        return _serviceProvider.GetRequiredService<T>();
    }
}
