using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Services.Abstractions;

internal interface ICamouflageService
{
    Task CreateAsync(string store, string table, string primaryKey, string column, string value);
    Task CreateAsync(object obj);
    Task<Uncamouflaged?> GetAsync(string store, string table, string primaryKey, string column);
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string column
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string primaryKey,
        string[] columns
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string[] columns
    );
}
