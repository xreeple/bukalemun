using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.Extensions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun;

internal sealed class Bukalemun(ICamouflageService _camouflageService) : IBukalemun
{
    public async Task CamouflageAsync(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    )
    {
        await _camouflageService.CreateAsync(store, tableName, primaryKey, columnName, value);
    }

    public async Task CamouflageAsync(object obj)
    {
        await _camouflageService.CreateAsync(obj);
    }

    public async Task<Uncamouflaged?> UncamouflageAsync(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    )
    {
        return await _camouflageService.GetAsync(store, tableName, primaryKey, columnName);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
    {
        return await _camouflageService.GetAsync(store, tableName, primaryKeys, columnName);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
    {
        return await _camouflageService.GetAsync(store, tableName, primaryKey, columnNames);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        return await _camouflageService.GetAsync(store, tableName, primaryKeys, columnNames);
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, tableName, primaryKeys, columnNames)).MapTo<T>();
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, tableName, primaryKey, columnNames)).MapTo<T>();
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, tableName, primaryKeys, columnName)).MapTo<T>();
    }

    public async Task<T> UncamouflageAsync<T>(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    )
        where T : new()
    {
        var uncamouflaged = await UncamouflageAsync(store, tableName, primaryKey, columnName);

        if (uncamouflaged is null)
        {
            return new T();
        }

        return uncamouflaged.MapTo<T>();
    }
}
