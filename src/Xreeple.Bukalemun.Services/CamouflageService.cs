using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Providers.Abstractions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Models;
using Xreeple.Bukalemun.Services.Options;

namespace Xreeple.Bukalemun.Services;

public class CamouflageService(
    IOptions<BukalemunOptions> _bukalemunOptions,
    ICamouflageRepository _camouflageRepository,
    ICryptoProvider _cryptoProvider
) : ICamouflageService
{
    public void Create(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    )
    {
        var encryptKey = _bukalemunOptions.Value.Stores[store].EncryptKey;
        var encrypted = _cryptoProvider.Encrypt(encryptKey, value);

        _camouflageRepository.Upsert(
            new Data.Entites.Camouflaged()
            {
                Store = store,
                TableName = tableName,
                PrimaryKey = primaryKey,
                ColumnName = columnName,
                Encrypted = encrypted,
            }
        );
    }
    public Uncamouflaged? Get(string store, string tableName, string primaryKey, string columnName)
    {
        return Get(store, tableName, [primaryKey], [columnName]).FirstOrDefault();
    }

    public IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
    {
        return Get(store, tableName, primaryKeys, [columnName]);
    }

    public IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
    {
        return Get(store, tableName, [primaryKey], columnNames);
    }

    public IEnumerable<Uncamouflaged> Get(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        var camouflaged = _camouflageRepository.Get(store, tableName, primaryKeys, columnNames);

        var result = camouflaged.Select(m => new Uncamouflaged()
        {
            Key = m.PrimaryKey,
            Name = m.ColumnName,
            Value = m.Encrypted is not null
                ? _cryptoProvider.Decrypt(
                    _bukalemunOptions.Value.Stores[store].EncryptKey,
                    m.Encrypted
                )
                : null,
        });

        return result;
    }
}
