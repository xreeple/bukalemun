using System.Text.Json.Nodes;
using Xreeple.Bukalemun.Services.Models;

namespace Xreeple.Bukalemun.Services.Abstractions;

public interface ICamouflageService
{
    void Create(string store, string tableName, string primaryKey, string columnName, string value);

    JsonNode CreateJson(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string fieldName,
        JsonNode json,
        bool autoKey
    );

    Uncamouflaged? Get(string store, string tableName, string primaryKey, string columnName);
    IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    );
    IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    );
    IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    );
}
