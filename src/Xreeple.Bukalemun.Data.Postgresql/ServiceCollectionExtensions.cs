using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xreeple.Bukalemun.AspNet;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Data.Postgresql.Repositories;

namespace Xreeple.Bukalemun.Data.Postgresql;
public static class ServiceCollectionExtensions
{
    private const string DefaultConnectionStringName = "DefaultConnection";
    private const string DefaultSchema = "bukalemun";

    public static BukalemunBuilder UseNpgsql(this BukalemunBuilder builder, string connectionStringName, string schema = DefaultSchema)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var connectionString = builder.Configuration.GetConnectionString(connectionStringName) ??
            throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");

        var dbContext = new PostgresqlDbContext(connectionString, schema);

        dbContext.Migration();

        builder.Services.AddSingleton<IDbContext>(_ => dbContext);

        builder.Services.AddScoped<ICamouflageRepository, CamouflageRepository>();

        return builder;
    }

    public static BukalemunBuilder UseNpgsql(this BukalemunBuilder builder)
    {
        return UseNpgsql(builder, DefaultConnectionStringName, DefaultSchema);
    }

    public static BukalemunBuilder UseNpgsql(this BukalemunBuilder builder, string schema)
    {
        return UseNpgsql(builder, DefaultConnectionStringName, schema);
    }
}
