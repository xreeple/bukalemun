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

    public JsonNode CreateJson(
        string store,
        string tableName,
        string primaryKey,
        string columnName,
        string fieldName,
        JsonNode json,
        bool autoKey
    )
    {
        var encryptKey = _bukalemunOptions.Value.Stores[store].EncryptKey;

        // Array alanı için: [key].name veya [].name
        var arrayFieldMatch = System.Text.RegularExpressions.Regex.Match(
            fieldName,
            @"\[(.*?)\]\.(.+)"
        );

        if (json is JsonArray arr1 && arr1.All(x => x is JsonValue) && autoKey)
        {
            var newArr = new JsonArray();
            foreach (var item in arr1)
            {
                var obj = new JsonObject
                {
                    ["value"] = item?.GetValue<string>(),
                    ["camouflageId"] = Guid.NewGuid().ToString(),
                };
                newArr.Add(obj);
            }
            return newArr;
        }
        else if (json is JsonArray arr && arrayFieldMatch.Success)
        {
            var keyField = arrayFieldMatch.Groups[1].Value; // "" (boş) veya "id", "identityNumber" vs.
            var targetField = arrayFieldMatch.Groups[2].Value;

            for (int i = 0; i < arr.Count; i++)
            {
                var item = arr[i] as JsonObject;
                if (item == null)
                    continue;

                // autoKey: her elemana camouflageId ekle
                if (autoKey)
                {
                    item["camouflageId"] = Guid.NewGuid().ToString();
                }

                if (string.IsNullOrEmpty(keyField))
                {
                    // [].name: index bazlı şifreleme
                    if (
                        item[targetField] is JsonValue val
                        && val.TryGetValue<string>(out var strVal)
                    )
                    {
                        var encrypted = _cryptoProvider.Encrypt(encryptKey, strVal);
                        //item[targetField] = Convert.ToBase64String(encrypted);
                    }
                }
                else
                {
                    // [key].name: keyField bazlı şifreleme
                    if (
                        item[keyField] is JsonValue keyVal
                        && keyVal.TryGetValue<string>(out var keyStr)
                    )
                    {
                        if (
                            item[targetField] is JsonValue val
                            && val.TryGetValue<string>(out var strVal)
                        )
                        {
                            var encrypted = _cryptoProvider.Encrypt(encryptKey, strVal);
                            //item[targetField] = Convert.ToBase64String(encrypted);
                        }
                    }
                }
            }
            return arr;
        }
        else if (json is JsonObject obj)
        {
            // Düz nesne için: fieldName doğrudan alan adı
            if (obj[fieldName] is JsonValue val && val.TryGetValue<string>(out var strVal))
            {
                var encrypted = _cryptoProvider.Encrypt(encryptKey, strVal);
                //obj[fieldName] = Convert.ToBase64String(encrypted);
            }
            return obj;
        }

        // Diğer durumlarda değişiklik yapmadan döndür
        return json;
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
