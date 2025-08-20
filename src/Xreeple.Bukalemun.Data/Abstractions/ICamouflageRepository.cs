using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Data.Abstractions;

internal interface ICamouflageRepository
{
    bool Upsert(Camouflaged camouflaged);
    Camouflaged? Get(string store, string tableName, string primaryKey, string columnName);
    IEnumerable<Camouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    );
    IEnumerable<Camouflaged> Get(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    );
    IEnumerable<Camouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    );
}
