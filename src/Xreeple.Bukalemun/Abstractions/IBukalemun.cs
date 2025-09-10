using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Abstractions;

/// <summary>
/// Provides methods to camouflage (encrypt) and uncamoﬂage (decrypt) data in a data store.
/// </summary>
public interface IBukalemun
{
    /// <summary>
    /// encrypts the specified value in the given store, table, and column identified by the primary key.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="key">The primary key identifying the record.</param>
    /// <param name="column">The column name to camouflage.</param>
    /// <param name="value">The value to camouflage (encrypt).</param>
    Task CamouflageAsync(string store, string table, string key, string column, string value);
    Task CamouflageAsync(object obj);

    /// <summary>
    /// Retrieves the decrypted value for the specified record and column.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="key">The primary key identifying the record.</param>
    /// <param name="column">The column name to uncamoﬂage (decrypt).</param>
    /// <returns>The decrypted value wrapped in an <see cref="Uncamouflaged"/> instance, or null if not found.</returns>
    Task<Uncamouflaged?> UncamouflageAsync(string store, string table, string key, string column);

    /// <summary>
    /// Retrieves decrypted values for multiple records and columns.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="keys">Array of primary keys identifying the records.</param>
    /// <param name="columns">Array of column names to uncamoﬂage (decrypt).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string[] keys,
        string[] columns
    );

    /// <summary>
    /// Retrieves decrypted values for multiple records with a single column.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="keys">Array of primary keys identifying the records.</param>
    /// <param name="column">The column name to uncamoﬂage (decrypt).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string[] keys,
        string column
    );

    /// <summary>
    /// Retrieves decrypted values for a single record with multiple columns.
    /// </summary>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="key">The primary key identifying the record.</param>
    /// <param name="columns">Array of column names to uncamoﬂage (decrypt).</param>
    /// <returns>A collection of <see cref="Uncamouflaged"/> instances.</returns>
    Task<IEnumerable<Uncamouflaged>> UncamouflageAsync(
        string store,
        string table,
        string key,
        string[] columns
    );

    /// <summary>
    /// Retrieves decrypted values mapped to instances of type <typeparamref name="T"/> for multiple records and columns.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="keys">Array of primary keys identifying the records.</param>
    /// <param name="columns">Array of column names to uncamoﬂage (decrypt).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string[] keys,
        string[] columns
    )
        where T : new();

    /// <summary>
    /// Retrieves decrypted values mapped to instances of type <typeparamref name="T"/> for a single record with multiple columns.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="key">The primary key identifying the record.</param>
    /// <param name="columns">Array of column names to uncamoﬂage (decrypt).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string key,
        string[] columns
    )
        where T : new();

    /// <summary>
    /// Retrieves decrypted values mapped to instances of type <typeparamref name="T"/> for multiple records with a single column.
    /// </summary>
    /// <typeparam name="T">The type to map the results to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="keys">Array of primary keys identifying the records.</param>
    /// <param name="column">The column name to uncamoﬂage (decrypt).</param>
    /// <returns>An enumerable of <typeparamref name="T"/> instances.</returns>
    Task<IEnumerable<T>> UncamouflageAsync<T>(
        string store,
        string table,
        string[] keys,
        string column
    )
        where T : new();

    /// <summary>
    /// Retrieves a single decrypted value mapped to an instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to map the result to. Must have a parameterless constructor.</typeparam>
    /// <param name="store">The data store identifier.</param>
    /// <param name="table">The table name containing the data.</param>
    /// <param name="key">The primary key identifying the record.</param>
    /// <param name="column">The column name to uncamoﬂage (decrypt).</param>
    /// <returns>An instance of <typeparamref name="T"/>.</returns>
    Task<T> UncamouflageAsync<T>(string store, string table, string key, string column)
        where T : new();
}
