using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Services.Abstractions;

internal interface ICamouflageService
{
    void Create(string store, string tableName, string primaryKey, string columnName, string value);
    Uncamouflaged? Get(string store, string tableName, string primaryKey, string columnName);
    IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    );
    IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    );
    IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    );
}
