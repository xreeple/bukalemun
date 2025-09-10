using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.Extensions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun;

internal sealed class Bukalemun(ICamouflageService _camouflageService) : IBukalemun
{
    public async Task CamouflageAsync(
        string store,
        string table,
        string primaryKey,
        string columnName,
        string value
    )
    {
        await _camouflageService.CreateAsync(store, table, primaryKey, columnName, value);
    }

    public async Task CamouflageAsync(object obj)
    {
        await _camouflageService.CreateAsync(obj);
    }

    public async Task<Uncamouflaged?> UncamouflageAsync(
        string store,
        string table,
        string primaryKey,
        string columnName
    )
    {
        return await _camouflageService.GetAsync(store, table, primaryKey, columnName);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string[] primaryKeys,
        string columnName
    )
    {
        return await _camouflageService.GetAsync(store, table, primaryKeys, columnName);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string primaryKey,
        string[] columnNames
    )
    {
        return await _camouflageService.GetAsync(store, table, primaryKey, columnNames);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        return await _camouflageService.GetAsync(store, table, primaryKeys, columnNames);
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string[] primaryKeys,
        string[] columnNames
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, primaryKeys, columnNames)).MapTo<T>();
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string primaryKey,
        string[] columnNames
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, primaryKey, columnNames)).MapTo<T>();
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string[] primaryKeys,
        string columnName
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, primaryKeys, columnName)).MapTo<T>();
    }

    public async Task<T> UncamouflageAsync<T>(
        string store,
        string table,
        string primaryKey,
        string columnName
    )
        where T : new()
    {
        var uncamouflaged = await UncamouflageAsync(store, table, primaryKey, columnName);

        if (uncamouflaged is null)
        {
            return new T();
        }

        return uncamouflaged.MapTo<T>();
    }
}
