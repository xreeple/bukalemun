using Microsoft.AspNetCore.Builder;

namespace Xreeple.Bukalemun.AspNet.Extensions;
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseBukalemun(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseExceptionHandler();
        app.UseStatusCodePages();

        return app;
    }
}