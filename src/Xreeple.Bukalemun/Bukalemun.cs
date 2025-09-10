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
        string column,
        string value
    )
    {
        await _camouflageService.CreateAsync(store, table, primaryKey, column, value);
    }

    public async Task CamouflageAsync(object obj)
    {
        await _camouflageService.CreateAsync(obj);
    }

    public async Task<Uncamouflaged?> UncamouflageAsync(
        string store,
        string table,
        string primaryKey,
        string column
    )
    {
        return await _camouflageService.GetAsync(store, table, primaryKey, column);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string[] primaryKeys,
        string column
    )
    {
        return await _camouflageService.GetAsync(store, table, primaryKeys, column);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string primaryKey,
        string[] columns
    )
    {
        return await _camouflageService.GetAsync(store, table, primaryKey, columns);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string[] primaryKeys,
        string[] columns
    )
    {
        return await _camouflageService.GetAsync(store, table, primaryKeys, columns);
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string[] primaryKeys,
        string[] columns
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, primaryKeys, columns)).MapTo<T>();
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string primaryKey,
        string[] columns
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, primaryKey, columns)).MapTo<T>();
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string[] primaryKeys,
        string column
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, primaryKeys, column)).MapTo<T>();
    }

    public async Task<T> UncamouflageAsync<T>(
        string store,
        string table,
        string primaryKey,
        string column
    )
        where T : new()
    {
        var uncamouflaged = await UncamouflageAsync(store, table, primaryKey, column);

        if (uncamouflaged is null)
        {
            return new T();
        }

        return uncamouflaged.MapTo<T>();
    }
}
