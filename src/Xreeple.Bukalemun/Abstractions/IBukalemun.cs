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

    Uncamouflaged? Uncamouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    );

    IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    );

    IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    );

    IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    );

    IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
        where T : new();

    IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
        where T : new();

    IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
        where T : new();

    T Uncamouflage<T>(string store, string tableName, string primaryKey, string columnName)
        where T : new();
}
