using Xreeple.Bukalemun.AspNet.Extensions;
using Xreeple.Bukalemun.Data.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddBukalemun(builder.Configuration)
    .UseNpgsql("app1");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseBukalemun();

app.UseHttpsRedirection();

app.Run();
