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
        camouflageService.Create("Default", "users", "2", "email", "john.doe@gmail.com");
        //var uncamouflaged = camouflageService.Get("Default", "users", "2", "name");

        var test = camouflageService.Get("Default", "users", ["1", "2"], ["name", "email"]);

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
