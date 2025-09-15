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
        string key,
        string column,
        string value
    )
    {
        await _camouflageService.CreateAsync(store, table, key, column, value);
    }

    public async Task CamouflageAsync<T>(T obj)
        where T : new()
    {
        await _camouflageService.CreateAsync(obj);
    }

    public async Task<T> UncamouflageAsync<T>(T obj)
        where T : new()
    {
        ArgumentNullException.ThrowIfNull(obj);

        return (await _camouflageService.GetAsync(obj)).MapTo(obj).First();
    }

    public async Task<Uncamouflaged?> UncamouflageAsync(
        string store,
        string table,
        string key,
        string column
    )
    {
        return await _camouflageService.GetAsync(store, table, key, column);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string[] keys,
        string column
    )
    {
        return await _camouflageService.GetAsync(store, table, keys, column);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string key,
        string[] columns
    )
    {
        return await _camouflageService.GetAsync(store, table, key, columns);
    }

    public async Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string[] keys,
        string[] columns
    )
    {
        return await _camouflageService.GetAsync(store, table, keys, columns);
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string[] keys,
        string[] columns
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, keys, columns)).MapTo<T>();
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string key,
        string[] columns
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, key, columns)).MapTo<T>();
    }

    public async Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string[] keys,
        string column
    )
        where T : new()
    {
        return (await UncamouflageAsync(store, table, keys, column)).MapTo<T>();
    }

    public async Task<T> UncamouflageAsync<T>(string store, string table, string key, string column)
        where T : new()
    {
        var uncamouflaged = await UncamouflageAsync(store, table, key, column);

        if (uncamouflaged is null)
        {
            return new T();
        }

        return uncamouflaged.MapTo<T>();
    }
}
