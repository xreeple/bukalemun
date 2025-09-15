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
        string table,
        string key,
        string column,
        string value
    )
    {
        var encryptKey = _bukalemunOptions.Value.Stores[store].EncryptKey;
        var encrypted = _cryptoProvider.Encrypt(encryptKey, value);

        await _camouflageRepository.UpsertAsync(
            new Data.Entites.Camouflaged()
            {
                Store = store,
                Table = table,
                Key = key,
                Column = column,
                Encrypted = encrypted,
            }
        );
    }

    public async Task<Uncamouflaged?> GetAsync(
        string store,
        string table,
        string key,
        string column
    )
    {
        return (await GetAsync(store, table, [key], [column])).FirstOrDefault();
    }

    public async Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string[] keys,
        string column
    )
    {
        return await GetAsync(store, table, keys, [column]);
    }

    public async Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string key,
        string[] columns
    )
    {
        return await GetAsync(store, table, [key], columns);
    }

    public async Task<IEnumerable<Uncamouflaged>> GetAsync(
        string store,
        string table,
        string[] keys,
        string[] columns
    )
    {
        var camouflaged = await _camouflageRepository.GetAsync(store, table, keys, columns);

        var result = camouflaged.Select(m => new Uncamouflaged()
        {
            Key = m.Key,
            Name = m.Column,
            Value = m.Encrypted is not null
                ? _cryptoProvider.Decrypt(
                    _bukalemunOptions.Value.Stores[store].EncryptKey,
                    m.Encrypted
                )
                : null,
        });

        return result;
    }

    public async Task<IEnumerable<Uncamouflaged>> GetAsync(object obj)
    {
        ExtractCamouflageMetadata(
            obj,
            out string store,
            out string table,
            out string key,
            out PropertyInfo[] camouflageableProperties
        );

        var columns = camouflageableProperties.Select(p => p.Name).ToArray();

        return await GetAsync(store, table, key, columns);
    }

    public async Task CreateAsync(object obj)
    {
        ExtractCamouflageMetadata(
            obj,
            out string store,
            out string table,
            out string key,
            out PropertyInfo[] camouflageableProperties
        );

        foreach (var camouflageableProperty in camouflageableProperties)
        {
            string column = camouflageableProperty.Name;
            string value =
                camouflageableProperty.GetValue(obj)?.ToString()
                ?? throw new NullReferenceException("The value cannot be null.");

            await CreateAsync(store, table, key, column, value);
        }
    }

    private static void ExtractCamouflageMetadata(
        object obj,
        out string store,
        out string table,
        out string key,
        out PropertyInfo[] camouflageableProperties
    )
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

        store = camouflageAttribute.Store;
        table = camouflageAttribute.Table == "default" ? type.Name : camouflageAttribute.Table;

        var properties = type.GetProperties().Where(p => p.CanRead);

        key = string.Join(
            "+",
            properties
                .Where(p => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute)))
                .OrderBy(p => p.GetCustomAttribute<PrimaryKeyAttribute>()?.Order ?? 0)
                .ToArray()
                .Select(p => p.GetValue(obj)?.ToString())
        );

        if (string.IsNullOrEmpty(key))
        {
            key =
                properties.FirstOrDefault(p => p.Name == "Id")?.GetValue(obj)?.ToString()
                ?? string.Empty;

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("The primary key is required.", nameof(obj));
            }
        }

        camouflageableProperties =
        [
            .. properties.Where(p =>
                p.CanWrite && Attribute.IsDefined(p, typeof(CamouflageableAttribute))
            ),
        ];
    }
}
