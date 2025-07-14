using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun;

public class Bukalemun(ICamouflageService _camouflageService) : IBukalemun
{
    public void Camouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    )
    {
        _camouflageService.Create(store, tableName, primaryKey, columnName, value);
    }

    public IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        return _camouflageService.Get(store, tableName, primaryKeys, columnNames);
    }
}
