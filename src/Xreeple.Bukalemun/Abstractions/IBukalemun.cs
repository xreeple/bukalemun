using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Abstractions;

public interface IBukalemun
{
    void Camouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    );
    IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    );
}
