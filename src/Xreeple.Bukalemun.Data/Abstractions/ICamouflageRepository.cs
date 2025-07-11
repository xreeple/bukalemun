using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Data.Abstractions;

public interface ICamouflageRepository
{
    bool Upsert(Camouflaged camouflaged);
    Camouflaged? Get(string store, string tableName, string primaryKey, string columnName);
}
