using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Services.Abstractions;

internal interface ICamouflageService
{
    Task CreateAsync(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    );
    Task<Uncamouflaged?> GetAsync(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    );
    Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    );
}
