using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xreeple.Bukalemun.Services;
using Xreeple.Bukalemun.Services.Abstractions;

namespace Xreeple.Bukalemun.AspNet.Extensions;
public static class ServiceCollectionExtensions
{
    public static BukalemunBuilder AddBukalemun(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddScoped<ICamouflageService, CamouflageService>();

        return new BukalemunBuilder(services, configuration);
    }
}
