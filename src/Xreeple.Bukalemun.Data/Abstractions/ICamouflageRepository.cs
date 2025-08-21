using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Data.Abstractions;

internal interface ICamouflageRepository
{
    Task<bool> UpsertAsync(Camouflaged camouflaged);
    Task<Camouflaged?> GetAsync(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    );
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    );
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    );
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    );
}
