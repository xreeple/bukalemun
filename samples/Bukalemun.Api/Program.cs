using Microsoft.AspNetCore.Mvc;
using Xreeple.Bukalemun.AspNet.Extensions;
using Xreeple.Bukalemun.Data.Postgresql;
using Xreeple.Bukalemun.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddBukalemun(builder.Configuration)
    .UseNpgsql("app1");

var app = builder.Build();


app.MapGet("/sample-1", ([FromServices] ICamouflageService camouflageService) =>
{
    camouflageService.Create("Default", "users", "1", "name", "John Doe");

    return "";
});


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseBukalemun();

app.UseHttpsRedirection();

app.Run();
