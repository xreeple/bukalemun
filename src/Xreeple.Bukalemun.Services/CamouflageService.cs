using Microsoft.Extensions.Options;
using Xreeple.Bukalemun.Data.Abstractions;
using Xreeple.Bukalemun.Providers.Abstractions;
using Xreeple.Bukalemun.Services.Abstractions;
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
}
