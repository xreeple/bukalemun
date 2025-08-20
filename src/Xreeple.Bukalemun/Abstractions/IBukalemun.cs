using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Abstractions;

/// <summary>
/// Provides methods to camouflage (mask) and uncamoﬂage (unmask) data in a data store.
/// </summary>
public interface IBukalemun
{
    /// <summary>
    /// Masks the specified value in the given store, table, and column identified by the primary key.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnName">The column name to camouflage.</param>
    /// <param name="value">The value to camouflage (mask).</param>
    void Camouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    );

    /// <summary>
    /// Retrieves the unmasked value for the specified record and column.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnName">The column name to uncamoﬂage (unmask).</param>
    /// <returns>The unmasked value wrapped in an <see cref="Uncamouflaged"/> instance, or null if not found.</returns>
    Uncamouflaged? Uncamouflage(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    );

    /// <summary>
    /// Retrieves unmasked values for multiple records and columns.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKeys">Array of primary keys identifying the records.</param>
    /// <param name="columnNames">Array of column names to uncamoﬂage (unmask).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    );

    /// <summary>
    /// Retrieves unmasked values for multiple records with a single column.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKeys">Array of primary keys identifying the records.</param>
    /// <param name="columnName">The column name to uncamoﬂage (unmask).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    );

    /// <summary>
    /// Retrieves unmasked values for a single record with multiple columns.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnNames">Array of column names to uncamoﬂage (unmask).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    IEnumerable<Uncamouflaged> Uncamouflage(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    );

    /// <summary>
    /// Retrieves unmasked values mapped to instances of type <typeparamref name="T"/> for multiple records and columns.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKeys">Array of primary keys identifying the records.</param>
    /// <param name="columnNames">Array of column names to uncamoﬂage (unmask).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
        where T : new();

    /// <summary>
    /// Retrieves unmasked values mapped to instances of type <typeparamref name="T"/> for a single record with multiple columns.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnNames">Array of column names to uncamoﬂage (unmask).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
        where T : new();

    /// <summary>
    /// Retrieves unmasked values mapped to instances of type <typeparamref name="T"/> for multiple records with a single column.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKeys">Array of primary keys identifying the records.</param>
    /// <param name="columnName">The column name to uncamoﬂage (unmask).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    IEnumerable<T> Uncamouflage<T>(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
        where T : new();

    /// <summary>
    /// Retrieves a single unmasked value mapped to an instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to map the result to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="tableName">The table name containing the data.</param>
    /// <param name="primaryKey">The primary key identifying the record.</param>
    /// <param name="columnName">The column name to uncamoﬂage (unmask).</param>
    /// <returns>An instance of <typeparamref name="T"/>.</returns>
    T Uncamouflage<T>(string store, string tableName, string primaryKey, string columnName)
        where T : new();
}
