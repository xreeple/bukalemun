using Microsoft.AspNetCore.Mvc;
using Xreeple.Bukalemun.AspNet.Extensions;
using Xreeple.Bukalemun.Postgresql;
using Xreeple.Bukalemun.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddBukalemun(builder.Configuration).UseNpgsql("app1");

var app = builder.Build();

app.MapGet(
    "/sample-1",
    ([FromServices] ICamouflageService camouflageService) =>
    {
        camouflageService.Create("Default", "users", "2", "name", "John Doe");
        var uncamouflaged = camouflageService.Get("Default", "users", "2", "name");

        return "";
    }
);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseBukalemun();

app.UseHttpsRedirection();

app.Run();
