using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Services.Abstractions;

internal interface ICamouflageService
{
    Task CreateAsync(
        string store,
        string table,
        string primaryKey,
        string columnName,
        string value
    );
    Task CreateAsync(object obj);
    Task<Uncamouflaged?> GetAsync(string store, string table, string primaryKey, string columnName);
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string columnName
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string primaryKey,
        string[] columnNames
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string[] columnNames
    );
}
