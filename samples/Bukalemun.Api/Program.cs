using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.AspNet.Extensions;
using Xreeple.Bukalemun.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddBukalemun(builder.Configuration).UseNpgsql();

var app = builder.Build();

app.MapGet(
    "/sample-1",
    ([FromServices] IBukalemun bukalemun) =>
    {
        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
            bukalemun.Camouflage("Default", "users", "2", "name", "John Doe");

            //var newJson = bukalemun.CamouflageJson("Default", "users", "2", "columnName", "fieldName -> [id].userName", json, false -> bool-> var olan key)
            //var newJson = bukalemun.CamouflageJson("Default", "users", "2", "columnName", "fieldName -> [].userName", json, true -> bool-> oto id)
            //var newJson = bukalemun.CamouflageJson("Default", "users", "2", "columnName", "fieldName -> userName", json, true -> bool-> oto id)

            scope.Complete();
        }

        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
            bukalemun.Camouflage("Default", "users", "2", "email", "john.doe@gmail.com");

            scope.Complete();
        }

        var users = bukalemun.Uncamouflage("Default", "users", ["1", "2"], ["name", "email"]);

        var test1 = bukalemun.Uncamouflage<BukalemunUser>(
            "Default",
            "users",
            ["1", "2"],
            ["name", "email"]
        );

        var test2 = bukalemun.Uncamouflage<BukalemunUser>(
            "Default",
            "users",
            "1",
            ["name", "email", "data[0].title"]
        );

        // Index base
        // posts[0].title
        // posts[1].title
        // posts[2].title
        // posts[0].comments[0].content
        // post.title
        // post.comments[0].content

        // Key base
        // posts[camouflageId=qweqwe-qweqweqwqwe-qweqweqwe] -> otomatik id
        // posts[id=1] -> nesnede halihazırda olan key

        var test3 = bukalemun.Uncamouflage<BukalemunUser>("Default", "users", ["1", "2"], "name");

        var test4 = bukalemun.Uncamouflage<BukalemunUser>("Default", "users", "2", "name");

        return test4;
    }
);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

public class BukalemunUser
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}

//[Camouflage(Store = "Users", TableName = "User")]
//public class User
//{
//    public int Id { get; set; }

//    [Camouflageable]
//    public string Name { get; set; }
//}
