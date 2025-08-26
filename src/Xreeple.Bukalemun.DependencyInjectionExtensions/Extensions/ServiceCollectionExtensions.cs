using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.DependencyInjectionExtensions;
using Xreeple.Bukalemun.Providers;
using Xreeple.Bukalemun.Providers.Abstractions;
using Xreeple.Bukalemun.Services;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Options;

namespace Xreeple.Bukalemun.DependencyInjectionExtensions.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Extension method to register Bukalemun services and options in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add the Bukalemun services to.</param>
    /// <param name="configuration">The application configuration containing Bukalemun settings.</param>
    /// <returns>A <see cref="BukalemunBuilder"/> to allow further Bukalemun configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    public static BukalemunBuilder AddBukalemun(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<BukalemunOptions>(configuration.GetSection("Bukalemun"));

        services.AddScoped<ICryptoProvider, CryptoProvider>();

        services.AddScoped<ICamouflageService, CamouflageService>();

        services.AddScoped<IBukalemun, Bukalemun>();

        return new BukalemunBuilder(services, configuration);
    }
}
