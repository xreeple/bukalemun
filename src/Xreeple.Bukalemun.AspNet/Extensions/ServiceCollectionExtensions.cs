using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xreeple.Bukalemun.Providers;
using Xreeple.Bukalemun.Providers.Abstractions;
using Xreeple.Bukalemun.Services;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Options;

namespace Xreeple.Bukalemun.AspNet.Extensions;
public static class ServiceCollectionExtensions
{
    public static BukalemunBuilder AddBukalemun(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<BukalemunOptions>(configuration.GetSection("Bukalemun"));

        services.AddScoped<ICryptoProvider, CryptoProvider>();

        services.AddScoped<ICamouflageService, CamouflageService>();

        return new BukalemunBuilder(services, configuration);
    }
}
