﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xreeple.Bukalemun.AspNet;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Postgresql.Repositories;

namespace Xreeple.Bukalemun.Postgresql;

public static class ServiceCollectionExtensions
{
    private const string DefaultConnectionStringName = "DefaultConnection";

    public static BukalemunBuilder UseNpgsql(
        this BukalemunBuilder builder,
        string connectionStringName,
        string appName
    )
    {
        ArgumentNullException.ThrowIfNull(builder);

        var connectionString =
            builder.Configuration.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{connectionStringName}' not found."
            );

        var dbContext = new PostgresqlDbContext(connectionString, "bukalemun." + appName.ToLower());

        var stores = builder
            .Configuration.GetSection("Bukalemun:Stores")
            .GetChildren()
            .Select(m => m.Key)
            .ToHashSet();

        dbContext.Migration(stores);

        builder.Services.AddSingleton<IDbContext>(_ => dbContext);

        builder.Services.AddScoped<ICamouflageRepository, CamouflageRepository>();

        return builder;
    }

    public static BukalemunBuilder UseNpgsql(this BukalemunBuilder builder, string appName)
    {
        return UseNpgsql(builder, DefaultConnectionStringName, appName);
    }
}
