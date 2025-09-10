using System.Reflection;
using Microsoft.Extensions.Options;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Providers.Abstractions;
using Xreeple.Bukalemun.Services.Abstractions;
using Xreeple.Bukalemun.Services.Attributes;
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

    public async Task CreateAsync(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var type = obj.GetType();

        if (!type.IsClass || type == typeof(string))
            throw new ArgumentException("Only class types can be processed.", nameof(obj));

        var camouflageAttribute =
            type.GetCustomAttribute<CamouflageAttribute>()
            ?? throw new ArgumentException(
                "The class must be decorated with [Camouflage].",
                nameof(obj)
            );

        string store = camouflageAttribute.Store;
        string tableName =
            camouflageAttribute.TableName == "default" ? type.Name : camouflageAttribute.TableName;

        var properties = type.GetProperties().Where(p => p.CanRead);

        string? primaryKey = string.Join(
            "",
            properties
                .Where(p => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute)))
                .OrderBy(p => p.GetCustomAttribute<PrimaryKeyAttribute>()?.Order ?? 0)
                .ToArray()
                .Select(p => p.GetValue(obj)?.ToString())
        );

        if (string.IsNullOrEmpty(primaryKey))
        {
            primaryKey = properties.FirstOrDefault(p => p.Name == "Id")?.GetValue(obj)?.ToString();

            if (string.IsNullOrEmpty(primaryKey))
            {
                throw new ArgumentException("The primary key is required.", nameof(obj));
            }
        }

        var camouflageableProperties = properties
            .Where(p => p.CanWrite && Attribute.IsDefined(p, typeof(CamouflageableAttribute)))
            .ToArray();

        foreach (var camouflageableProperty in camouflageableProperties)
        {
            string columnName = camouflageableProperty.Name;
            string value =
                camouflageableProperty.GetValue(obj)?.ToString()
                ?? throw new NullReferenceException("The value cannot be null.");

            await CreateAsync(store, tableName, primaryKey, columnName, value);
        }
    }
}
