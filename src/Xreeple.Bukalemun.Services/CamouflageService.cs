using Microsoft.Extensions.Options;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Providers.Abstractions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Models;
using Xreeple.Bukalemun.Services.Options;

namespace Xreeple.Bukalemun.Services;

internal sealed class CamouflageService(
    IOptions<BukalemunOptions> _bukalemunOptions,
    ICamouflageRepository _camouflageRepository,
    ICryptoProvider _cryptoProvider
) : ICamouflageService
{
    public async Task CreateAsync(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string value
    )
    {
        var encryptKey = _bukalemunOptions.Value.Stores[store].EncryptKey;
        var encrypted = _cryptoProvider.Encrypt(encryptKey, value);

        await _camouflageRepository.UpsertAsync(
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

    public async Task<Uncamouflaged?> GetAsync(
        string store,
        string tableName,
        string primaryKey,
        string columnName
    )
    {
        return (await GetAsync(store, tableName, [primaryKey], [columnName])).FirstOrDefault();
    }

    public async Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string columnName
    )
    {
        return await GetAsync(store, tableName, primaryKeys, [columnName]);
    }

    public async Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string tableName,
        string primaryKey,
        string[] columnNames
    )
    {
        return await GetAsync(store, tableName, [primaryKey], columnNames);
    }

    public async Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string tableName,
        string[] primaryKeys,
        string[] columnNames
    )
    {
        var camouflaged = await _camouflageRepository.GetAsync(
            store,
            tableName,
            primaryKeys,
            columnNames
        );

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
