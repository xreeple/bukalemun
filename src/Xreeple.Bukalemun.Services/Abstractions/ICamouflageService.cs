using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Services.Abstractions;

internal interface ICamouflageService
{
    Task CreateAsync(string store, string table, string key, string column, string value);
    Task CreateAsync<T>(T obj)
        where T : new();
    Task<Uncamouflaged?> GetAsync(string store, string table, string key, string column);
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string[] keys,
        string column
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string key,
        string[] columns
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string[] keys,
        string[] columns
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync<T>(T obj)
        where T : new();
}
