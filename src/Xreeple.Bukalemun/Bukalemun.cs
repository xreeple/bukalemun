using System.Text.Json;
using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.Extensions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun;

public class Bukalemun(ICamouflageService _camouflageService) : IBukalemun
{
    public void Camouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    )
    {
        _camouflageService.Create(store, tableName, primaryKey, columnName, value);

        var obj = new { Name = "Mehmet" };

        var listObj1 = new List<object> { new { Name = "Mehmet" }, new { Name = "Halime" } };
        var listObj2 = new List<object> { "Mehmet", "Halime" };

        var json = JsonSerializer.SerializeToNode(listObj2);

        var test = _camouflageService.CreateJson(
            store,
            tableName,
            primaryKey,
            columnName,
            "[]",
            json,
            true
        );
    }

    public Uncamouflaged? Uncamouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    )
    {
        return _camouflageService.Get(store, tableName, primaryKey, columnName);
    }

    public IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
    {
        return _camouflageService.Get(store, tableName, primaryKeys, columnName);
    }

    public IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
    {
        return _camouflageService.Get(store, tableName, primaryKey, columnNames);
    }

    public IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        return _camouflageService.Get(store, tableName, primaryKeys, columnNames);
    }

    public IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
        where T : new()
    {
        return Uncamouflage(store, tableName, primaryKeys, columnNames).MapTo<T>();
    }

    public IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
        where T : new()
    {
        return Uncamouflage(store, tableName, primaryKey, columnNames).MapTo<T>();
    }

    public IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
        where T : new()
    {
        return Uncamouflage(store, tableName, primaryKeys, columnName).MapTo<T>();
    }

    public T Uncamouflage<T>(string store, string tableName, string primaryKey, string columnName)
        where T : new()
    {
        var uncamouflaged = Uncamouflage(store, tableName, primaryKey, columnName);

        if (uncamouflaged is null)
        {
            return new T();
        }

        return uncamouflaged.MapTo<T>();
    }
}
