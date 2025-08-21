using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.AspNet.Extensions;
using Xreeple.Bukalemun.Masking;
using Xreeple.Bukalemun.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddBukalemun(builder.Configuration).UseNpgsql();

var app = builder.Build();

app.MapGet(
    "/sample-1",
    async ([FromServices] IBukalemun bukalemun) =>
    {
        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
            await bukalemun.CamouflageAsync("Default", "users", "2", "name", "John Doe");
            scope.Complete();
        }

        using (var scope = new TransactionScope(TransactionScopeOption.Required))
        {
            await bukalemun.CamouflageAsync("Default", "users", "2", "email", "john.doe@gmail.com");

            scope.Complete();
        }

        var users = await bukalemun.UncamouflageAsync(
            "Default",
            "users",
            ["1", "2"],
            ["name", "email"]
        );

        var test1 = await bukalemun.UncamouflageAsync<BukalemunUser>(
            "Default",
            "users",
            ["1", "2"],
            ["name", "email"]
        );

        var test3 = await bukalemun.UncamouflageAsync<BukalemunUser>(
            "Default",
            "users",
            ["1", "2"],
            "name"
        );

        var test4 = await bukalemun.UncamouflageAsync<BukalemunUser>(
            "Default",
            "users",
            "2",
            "name"
        );

        // 1. RevealFirst
        Console.WriteLine(Mask.Build("helloworld").RevealFirst(2).ToString());
        // he********

        // 2. RevealLast
        Console.WriteLine(Mask.Build("helloworld").RevealLast(3).ToString());
        // *******rld

        // 3. RevealRange
        Console.WriteLine(Mask.Build("helloworld").RevealRange(3, 2).ToString());
        // ***lo*****

        // 4. RevealRegex
        Console.WriteLine(Mask.Build("ahmet@gmail.com").RevealRegex(@"@.*$").ToString());
        // *****@gmail.com

        // 5. RevealIf
        Console.WriteLine(
            Mask.Build("abc123xyz").RevealIf((ch, idx) => char.IsDigit(ch)).ToString()
        );
        // ***123***

        // 6. PreserveChars
        Console.WriteLine(
            Mask.Build("1234-5678-9012-1234").PreserveChars("-").RevealLast(4).ToString()
        );
        // ****-****-****-1234

        // 7. PreserveWhitespace
        Console.WriteLine(Mask.Build("555 123 4567").RevealLast(2).PreserveWhitespace().ToString());
        // *** *** ***67

        // 8. MaskChar
        Console.WriteLine(Mask.Build("helloworld").RevealLast(3).MaskChar('#').ToString());
        // #######rld

        // 9a. CompactMask with RevealFirst
        Console.WriteLine(Mask.Build("helloworld").RevealFirst(2).CompactMask(3).ToString());
        // he***

        // 9b. CompactMask with RevealLast
        Console.WriteLine(Mask.Build("helloworld").RevealLast(2).CompactMask(3).ToString());
        // ***ld

        // 9c. CompactMask with RevealRange
        Console.WriteLine(Mask.Build("helloworld").RevealRange(3, 2).CompactMask(4).ToString());
        // ***lo****

        // 10. Kombine kullanım
        Console.WriteLine(
            Mask.Build("TR1200062001190000066728")
                .RevealFirst(4)
                .RevealLast(2)
                .MaskChar('#')
                .ToString()
        );
        // TR####################28

        Console.WriteLine(
            Mask.Build("mehmet emin eker")
                .RevealInitialsPerWord()
                .PreserveWhitespace()
                .MaskChar('#')
                .CompactMask(4)
                .ToString()
        );

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
