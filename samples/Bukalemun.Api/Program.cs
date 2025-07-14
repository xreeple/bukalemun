using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.AspNet.Extensions;
using Xreeple.Bukalemun.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddBukalemun(builder.Configuration).UseNpgsql("app1");

var app = builder.Build();

app.MapGet(
    "/sample-1",
    ([FromServices] IBukalemun bukalemun) =>
    {
        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
            bukalemun.Camouflage("Default", "users", "2", "name", "John Doe");

            scope.Complete();
        }

        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
            bukalemun.Camouflage("Default", "users", "2", "email", "john.doe@gmail.com");

            scope.Complete();
        }

        var users = bukalemun.Uncamouflage("Default", "users", ["1", "2"], ["name", "email"]);

        var test = bukalemun.Uncamouflage<BukalemunUser>(
            "Default",
            "users",
            ["1", "2"],
            ["name", "email"]
        );

        return test;
    }
);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseBukalemun();

app.UseHttpsRedirection();

app.Run();

public class BukalemunUser
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}
