using Xreeple.Bukalemun.Data.Entites;

namespace Xreeple.Bukalemun.Data.Abstractions;

internal interface ICamouflageRepository
{
    Task<bool> UpsertAsync(Camouflaged camouflaged);
    Task<bool> InsertAsync(Camouflaged camouflaged);
    Task<Camouflaged?> GetAsync(string store, string table, string key, string column);
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] keys,
        string column
    );
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string key,
        string[] columns
    );
    Task<IEnumerable<Camouflaged>> GetAsync(
        string store,
        string table,
        string[] keys,
        string[] columns
    );
}
