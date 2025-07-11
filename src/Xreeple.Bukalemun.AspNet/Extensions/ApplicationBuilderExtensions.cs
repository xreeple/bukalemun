using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xreeple.Bukalemun.Services.Options;

namespace Xreeple.Bukalemun.AspNet.Extensions;
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseBukalemun(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        using var scope = app.ApplicationServices.CreateScope();

        var opt = scope.ServiceProvider.GetRequiredService<IOptions<BukalemunOptions>>();

        return app;
    }
}