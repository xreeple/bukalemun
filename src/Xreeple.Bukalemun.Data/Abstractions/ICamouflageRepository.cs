using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Data.Abstractions;

internal interface ICamouflageRepository
{
    Task<bool> UpsertAsync(Camouflaged camouflaged);
    Task<Camouflaged?> GetAsync(string store, string table, string primaryKey, string column);
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string column
    );
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string primaryKey,
        string[] columns
    );
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] primaryKeys,
        string[] columns
    );
}
