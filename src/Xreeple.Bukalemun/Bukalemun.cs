using System.Text.Json;
using Xreeple.Bukalemun.Abstractions;
using Xreeple.Bukalemun.Extensions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun;

/// <summary>
/// Implementation of <see cref="IBukalemun"/> that uses <see cref="ICamouflageService"/> to mask and unmask data.
/// </summary>
public class Bukalemun(ICamouflageService _camouflageService) : IBukalemun
{
    /// <summary>
    /// Masks the specified value in the data store by delegating to <see cref="ICamouflageService.Create"/>.
    /// Also demonstrates JSON serialization and usage of <see cref="ICamouflageService.CreateJson"/>.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnName">The column name to camouflage.</param>
    /// <param name="value">The value to camouflage (mask).</param>
    public void Camouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    )
    {
        _camouflageService.Create(store, tableName, primaryKey, columnName, value);

        var obj = new { Name = "Mehmet" };

        var listObj1 = new List<object> { new { Name = "Mehmet" }, new { Name = "Halime" } };
        var listObj2 = new List<object> { "Mehmet", "Halime" };

        var json = JsonSerializer.SerializeToNode(listObj2);

        var test = _camouflageService.CreateJson(
            store,
            tableName,
            primaryKey,
            columnName,
            "[]",
            json,
            true
        );
    }

    /// <summary>
    /// Retrieves the unmasked value for the specified record and column via <see cref="ICamouflageService.Get"/>.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnName">The column name to uncamoﬂage (unmask).</param>
    /// <returns>The unmasked value wrapped in an <see cref="Uncamouflaged"/> instance, or null if not found.</returns>
    public Uncamouflaged? Uncamouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    )
    {
        return _camouflageService.Get(store, tableName, primaryKey, columnName);
    }

    public IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
    {
        return _camouflageService.Get(store, tableName, primaryKeys, columnName);
    }

    /// <summary>
    /// Retrieves unmasked values for multiple records and a single column.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKeys">Array of primary keys identifying the records.</param>
    /// <param name="columnName">The column name to uncamoﬂage (unmask).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    public IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
    {
        return _camouflageService.Get(store, tableName, primaryKey, columnNames);
    }

    /// <summary>
    /// Retrieves unmasked values for a single record and multiple columns.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnNames">Array of column names to uncamoﬂage (unmask).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    public IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        return _camouflageService.Get(store, tableName, primaryKeys, columnNames);
    }

    /// <summary>
    /// Retrieves unmasked values for multiple records and multiple columns.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKeys">Array of primary keys identifying the records.</param>
    /// <param name="columnNames">Array of column names to uncamoﬂage (unmask).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    public IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
        where T : new()
    {
        return Uncamouflage(store, tableName, primaryKeys, columnNames).MapTo<T>();
    }

    /// <summary>
    /// Retrieves unmasked values mapped to instances of type <typeparamref name="T"/> for multiple records and multiple columns.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKeys">Array of primary keys identifying the records.</param>
    /// <param name="columnNames">Array of column names to uncamoﬂage (unmask).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    public IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
        where T : new()
    {
        return Uncamouflage(store, tableName, primaryKey, columnNames).MapTo<T>();
    }

    /// <summary>
    /// Retrieves unmasked values mapped to instances of type <typeparamref name="T"/> for a single record and multiple columns.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnNames">Array of column names to uncamoﬂage (unmask).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    public IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
        where T : new()
    {
        return Uncamouflage(store, tableName, primaryKeys, columnName).MapTo<T>();
    }

    /// <summary>
    /// Retrieves unmasked values mapped to instances of type <typeparamref name="T"/> for multiple records and a single column.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKeys">Array of primary keys identifying the records.</param>
    /// <param name="columnName">The column name to uncamoﬂage (unmask).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    public T Uncamouflage<T>(string store, string tableName, string primaryKey, string columnName)
        where T : new()
    {
        var uncamouflaged = Uncamouflage(store, tableName, primaryKey, columnName);

        if (uncamouflaged is null)
        {
            return new T();
        }

        return uncamouflaged.MapTo<T>();
    }
}
