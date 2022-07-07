using AEXMovies.Services.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace AEXMovies.Tests.Unit;

public class BaseTest
{
    private readonly ServiceCollection _serviceCollection = new();
    private ServiceProvider? _serviceProvider = null;
    private readonly List<Action<ServiceProvider>> _serviceProviderHooks = new();

    public BaseTest()
    {
        RegisterServices(collection =>
        {
            collection.AddAutoMapper(typeof(DtoAutoMapperProfile));
        });
    }

    protected void RegisterServices(Action<ServiceCollection> servicesBuilder)
    {
        if (_serviceProvider != null)
            throw new InvalidOperationException("Service provider is already built!");
        servicesBuilder(_serviceCollection);
    }

    protected ServiceProvider BuildOrGetServiceProvider()
    {
        if (_serviceProvider == null)
        {
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            foreach (var hook in _serviceProviderHooks)
            {
                hook(_serviceProvider);
            }
        }
        return _serviceProvider;
    }

    protected void OnServiceProvider(Action<ServiceProvider> provider)
    {
        _serviceProviderHooks.Add(provider);
    }
}